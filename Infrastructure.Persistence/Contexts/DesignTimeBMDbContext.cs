using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design; 
namespace Infrastructure.Persistence
{
    public class DesignTimeBMDbContext : IDesignTimeDbContextFactory<Infrastructure.Persistence.ApplicationDbContext>
    {
        public Infrastructure.Persistence.ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Infrastructure.Persistence.ApplicationDbContext>(); 
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));
            return new Infrastructure.Persistence.ApplicationDbContext(optionsBuilder.Options);
        }
    }
}