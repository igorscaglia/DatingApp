using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    // No Core > 3.0 (No Microsoft.AspNetCore.App) foram removidos os assemblies do EF. 
    // A referência deve ser feita manualmente pelo Nuget.
    public class SqlServerDataContext : DbContext
    {
        protected SqlServerDataContext() { }

        public SqlServerDataContext(DbContextOptions<SqlServerDataContext> options) : base(options) { }

        // Abordagem Code First
        public DbSet<Value> Values { get; set; }
    }
}