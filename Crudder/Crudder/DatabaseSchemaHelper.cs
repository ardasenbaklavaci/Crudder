using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;

public class DatabaseSchemaHelper
{
    private readonly DynamicDbContext _dbContext;

    public DatabaseSchemaHelper(DynamicDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<string> GetTablesFromDatabase()
    {
        var tables = new List<string>();

        using (var connection = _dbContext.Database.GetDbConnection())
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                command.CommandType = System.Data.CommandType.Text;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }
        }

        return tables;
    }
}
