using System.IO;
using AutoMapper.Configuration;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Process
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    var site = hostContext.Configuration.GetConnectionString("DefaultConnection");
                    optionsBuilder.UseMySql(hostContext.Configuration.GetConnectionString("DefaultConnection"));
                    services.AddScoped<ApplicationDbContext>(s => new ApplicationDbContext(optionsBuilder.Options));
                    services.AddHostedService<MailWorker>();
                });
    }
}
