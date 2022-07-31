using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql;  

namespace WebApi
{
    public class DesignTimeApplicationDbContext : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = "Server=139.59.136.218; Database=adler; Uid=root; Pwd=Adler()develop0";
            optionsBuilder.UseMySQL(connectionString, x => x.MigrationsAssembly("Infrastructure.Persistence")).EnableSensitiveDataLogging();

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
