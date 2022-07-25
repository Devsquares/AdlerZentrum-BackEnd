using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Configuration;

namespace WebApi
{
   public class DesignTimeApplicationDbContext : IDesignTimeDbContextFactory<ApplicationDbContext>
    { 
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>(); 
            optionsBuilder.UseMySQL("Server=46.101.185.132;Database=adler;Uid=root;Pwd=Adler()develop", x => x.MigrationsAssembly("Infrastructure.Persistence")).EnableSensitiveDataLogging();
           
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
