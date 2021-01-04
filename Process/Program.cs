using AutoMapper.Configuration;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Process
{
    public class Program
    {
        public IConfiguration Configuration { get; }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    optionsBuilder.UseMySql("Server=MYSQL5035.site4now.net;Database=db_a681e3_adler;Uid=a681e3_adler;Pwd=adler0db");
                    services.AddScoped<ApplicationDbContext>(s => new ApplicationDbContext(optionsBuilder.Options));
                    services.AddHostedService<HomeworkCorrectionWorker>();
                });
    }
}
