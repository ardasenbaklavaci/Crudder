using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

public class DynamicDbContext : DbContext
{
    public DynamicDbContext(DbContextOptions<DynamicDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Get the assembly containing the dynamically generated models
        var modelsAssembly = Assembly.GetExecutingAssembly();

        // Filter types that are classes, public, not abstract, and in the 'DynamicModels' namespace
        var entityTypes = modelsAssembly.GetTypes()
            .Where(t => t.IsClass && t.IsPublic && !t.IsAbstract && t.Namespace == "DynamicModels");

        foreach (var type in entityTypes)
        {
            modelBuilder.Entity(type);
        }
    }
}
