using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Configuration;

namespace WebApi
{
   public class DesignTimeApplicationDbContext : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public IConfiguration _config { get; }
       public DesignTimeApplicationDbContext(IConfiguration configuration){
            _config = configuration;
        }
        
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>(); 
            optionsBuilder.UseMySQL(_config.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("Infrastructure.Persistence")).EnableSensitiveDataLogging();
           
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
