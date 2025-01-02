using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

String connectionString = "";

// Configure DbContexts for multiple databases
builder.Services.AddDbContext<DynamicDbContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    connectionString = configuration.GetConnectionString("DefaultConnection");

    var connectionString2 = configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString2);
});

builder.Services.AddScoped<DatabaseSchemaHelper>();
var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var scopedServices = scope.ServiceProvider;
    var databaseHelper = scopedServices.GetRequiredService<DatabaseSchemaHelper>();
    var tables = databaseHelper.GetTablesFromDatabase();
    var generator = new RazorPageGenerator(connectionString);

    /*foreach (String table in tables)
    {
        generator.GenerateCrudPages(table, "Pages");
    }*/

    String baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    string solutionDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\..\"));

    string targetDirectory = Path.Combine(solutionDirectory, "CoreProject");
    targetDirectory = Path.Combine(targetDirectory, "Pages");

    //generator.GenerateCrudPages(tables, "Pages");

    generator.GenerateCrudPages(tables, targetDirectory);
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
