
using Microsoft.EntityFrameworkCore;
using Models;
using Data;

namespace Data
{
    public class NewDbContext : DbContext
    {
        public NewDbContext(DbContextOptions<NewDbContext> options)
            : base(options)
        { }

        public DbSet<Customers> Customers { get; set; }
        public DbSet<Products> Products { get; set; }
    }
}