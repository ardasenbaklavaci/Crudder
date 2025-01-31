﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class RazorPageGenerator
{
    private readonly string _connectionString;
    private readonly DynamicTypeGenerator _dynamicTypeGenerator;

    public RazorPageGenerator(string connectionString)
    {
        _connectionString = connectionString;
        _dynamicTypeGenerator = new DynamicTypeGenerator();  // Initialize the dynamic type generator
    }

    public void GenerateCrudPages(List<string> tableNames, string outputDirectory)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string solutionDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\"));  // Adjust based on your solution structure

        foreach (var tableName in tableNames)
        {
            var pageName = tableName;
            var modelName = $"{pageName}Model";
            var pageDirectory = Path.Combine(outputDirectory, pageName);

            Directory.CreateDirectory(pageDirectory);

            GenerateIndexPage(tableName, pageDirectory);

            // Generate Razor Page (HTML)
            var razorPageContent = $@"
@page
@model {modelName}

<h2>{pageName}</h2>

<form method=""post"">
    <!-- Form fields for {pageName} -->
    <button type=""submit"">Save</button>
</form>
            ";

            File.WriteAllText(Path.Combine(pageDirectory, $"{pageName}.cshtml"), razorPageContent);

            // Generate Page Model (C#)
            var pageModelContent = $@"
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Data;

public class {modelName} : PageModel
{{
    private readonly NewDbContext _context;

    public {modelName}(NewDbContext context)
    {{
        _context = context;
    }}

    public List<{pageName}> {pageName} {{ get; set; }}

    public async Task OnGetAsync()
    {{
        {pageName} = await _context.{pageName}.ToListAsync();
    }}
}}";

            File.WriteAllText(Path.Combine(pageDirectory, $"{modelName}.cs"), pageModelContent);

            // Generate Model C# Class dynamically using DynamicTypeGenerator
            var modelClassContent = GenerateModelClass(tableName);
            var modelsDirectory = Path.Combine(outputDirectory, "Models");
            Directory.CreateDirectory(modelsDirectory);
            File.WriteAllText(Path.Combine(modelsDirectory, $"{tableName}.cs"), modelClassContent);

            // Create dynamic model type using DynamicTypeGenerator
            var columns = GetTableSchema(tableName);
            Type dynamicModelType = _dynamicTypeGenerator.CreateDynamicType(tableName, columns);
            // Optionally: You can create dynamic instances of the model here or map them to a DbContext
        }

        // Generate NewDbContext Class (C#)
        var dbContextContent = $@"
using Microsoft.EntityFrameworkCore;
using Models;
using Data;

namespace Data
{{
    public class NewDbContext : DbContext
    {{
        public NewDbContext(DbContextOptions<NewDbContext> options)
            : base(options)
        {{ }}

        {string.Join("\n        ", tableNames.ConvertAll(table => $"public DbSet<{table}> {table} {{ get; set; }}"))}
    }}
}}";

        var dataDirectory = Path.Combine(outputDirectory, "Data");
        Directory.CreateDirectory(dataDirectory);
        File.WriteAllText(Path.Combine(dataDirectory, "NewDbContext.cs"), dbContextContent);

        
        


        String bdir = AppDomain.CurrentDomain.BaseDirectory;
        string dire = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\..\"));

        string targetDirectory = Path.Combine(dire, "CoreProject");

        UpdateAppSettings(targetDirectory);
    }
    private string GenerateModelClass(string tableName)
    {
        var properties = GetTableSchema(tableName);
        var primaryKeyColumn = GetPrimaryKeyColumn(tableName); // Get the primary key column for the table

        string propertyDefinitions = string.Join(Environment.NewLine, properties.ConvertAll(prop =>
        {
            string attribute = "";

            // Add [Key] attribute for the primary key column
            if (prop.ColumnName == primaryKeyColumn)
            {
                attribute += "        [Key]\n";
            }

            // Add [Required] attribute for string properties with "name" in the column name
            if (prop.DataType == "string" && prop.ColumnName.ToLower().Contains("name"))
            {
                attribute += "        [Required]\n";
            }

            return $"{attribute}        public {prop.DataType} {prop.ColumnName} {{ get; set; }}";
        }));

        return $@"
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using Data;

namespace Models
{{
    public class {tableName}
    {{
{propertyDefinitions}
    }}
}}";
    }
    private List<(string ColumnName, string DataType)> GetTableSchema(string tableName)
    {
        var columns = new List<(string ColumnName, string DataType)>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand($@"
                SELECT COLUMN_NAME, DATA_TYPE 
                FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = @TableName", connection))
            {
                command.Parameters.AddWithValue("@TableName", tableName);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string columnName = reader["COLUMN_NAME"].ToString();
                        string sqlDataType = reader["DATA_TYPE"].ToString();
                        string csharpDataType = MapSqlToCSharpType(sqlDataType);
                        columns.Add((columnName, csharpDataType));
                    }
                }
            }
        }

        return columns;
    }
    private string MapSqlToCSharpType(string sqlDataType)
    {
        return sqlDataType switch
        {
            "int" => "int",
            "bigint" => "long",
            "smallint" => "short",
            "bit" => "bool",
            "decimal" or "numeric" => "decimal",
            "float" => "double",
            "real" => "float",
            "date" or "datetime" or "smalldatetime" => "DateTime",
            "char" or "varchar" or "nvarchar" or "text" => "string",
            "binary" or "varbinary" or "image" or "timestamp" => "byte[]",  // Added 'timestamp' case
            "xml" => "XmlText",
            _ => "object"
        };
    }
    private string GetPrimaryKeyColumn(string tableName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand($@"
            SELECT COLUMN_NAME
            FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
            WHERE TABLE_NAME = @TableName AND OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1", connection))
            {
                command.Parameters.AddWithValue("@TableName", tableName);
                return command.ExecuteScalar()?.ToString();
            }
        }
    }
    private void UpdateAppSettings(string solutionDirectory)
    {
        var TargetAppSettingsPath = Path.Combine(solutionDirectory, "appsettings.json");
        JObject TargetAppSettings;

        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        JObject ResourceAppSettings;

        String bdir = AppDomain.CurrentDomain.BaseDirectory;
        string dire = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\..\"));

        var ResourcePath = Path.Combine(dire, "Crudder");
        var ResourceAppSettingsPath = Path.Combine(ResourcePath, "appsettings.json");

        if (File.Exists(TargetAppSettingsPath))
        {
            var targetJson = File.ReadAllText(TargetAppSettingsPath);
            TargetAppSettings = JObject.Parse(targetJson);

            if (File.Exists(ResourceAppSettingsPath))
            {
                var json = File.ReadAllText(ResourceAppSettingsPath);
                ResourceAppSettings = JObject.Parse(json);
            }

        }
        else
        {
            ResourceAppSettings = new JObject();
            TargetAppSettings = new JObject();
        }

        if (TargetAppSettings["ConnectionStrings"] == null)
        {
            TargetAppSettings["ConnectionStrings"] = new JObject();
        }

        TargetAppSettings["ConnectionStrings"]["DefaultConnection"] = _connectionString;

        File.WriteAllText(TargetAppSettingsPath, TargetAppSettings.ToString());
    }

    public void GenerateIndexPage(string tableName, string outputDirectory)
    {
        var pageName = tableName;
        var modelName = $"{pageName}IndexModel";
        //var pageDirectory = Path.Combine(outputDirectory, pageName);
        var pageDirectory = outputDirectory;

        // Ensure directory exists
        Directory.CreateDirectory(pageDirectory);

        // Generate Razor Index Page (HTML)
        var razorPageContent = $@"
@page
@model {modelName}

<h2>{pageName} List</h2>

<table class=""table"">
    <thead>
        <tr>
            @foreach (var column in Model.Columns)
            {{
    
                    <th> @column </th>
    
                }}
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model.
        {pageName})
        
        {{


                <tr>
                    @foreach(var column in Model.Columns)
                {{
                    <td>@item.GetType().GetProperty(column)?.GetValue(item, null)</td>
                }}
                <td>
                    <a asp-page=""Edit"" asp-route-id="""" class=""btn btn-primary"">Edit</a>
                    <a asp-page=""Details"" asp-route-id="""" class=""btn btn-info"">Details</a>
                    <a asp-page=""Delete"" asp-route-id="""" class=""btn btn-danger"">Delete</a>
                </td>
            </tr>
        }}
    </tbody>
</table>

<a asp-page=""Create"" class=""btn btn-success"">Create New</a>
";

        File.WriteAllText(Path.Combine(pageDirectory, $"{pageName}Index.cshtml"), razorPageContent);

        // Generate Page Model (C#)
        var pageModelContent = $@"
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Data;

public class {modelName} : PageModel
{{
    private readonly NewDbContext _context;

    public {modelName}(NewDbContext context)
    {{
        _context = context;
    }}

    public List<{pageName}> {pageName} {{ get; set; }}
    public List<string> Columns {{ get; set; }}

    public async Task OnGetAsync()
    {{
        {pageName} = await _context.{pageName}.ToListAsync();
        Columns = new List<string>();

        foreach (var prop in typeof({pageName}).GetProperties())
        {{
            Columns.Add(prop.Name);
        }}
    }}
}}";

        File.WriteAllText(Path.Combine(pageDirectory, $"{pageName}IndexModel.cs"), pageModelContent);
    }



}

