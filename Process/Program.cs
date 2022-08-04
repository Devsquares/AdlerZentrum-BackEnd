using System.IO;
using Application.Interfaces;
using Domain.Settings;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;

namespace Process
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // var config = new ConfigurationBuilder()
            // .AddJsonFile("appsettings.json")
            // .Build();

            // Log.Logger = new LoggerConfiguration()
            // .ReadFrom.Configuration(config)
            // .CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                    var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

                    optionsBuilder.UseMySql(hostContext.Configuration.GetConnectionString("DefaultConnection"), serverVersion);
                    services.Configure<MailSettings>(hostContext.Configuration.GetSection("MailSettings"));
                    services.AddTransient<IEmailService, EmailService>();
                    services.AddScoped<ApplicationDbContext>(s => new ApplicationDbContext(optionsBuilder.Options));
                    services.AddHostedService<MailWorker>();
                    services.AddHostedService<JobsWorker>();
                    services.AddHostedService<BanWorker>();
                    services.AddHostedService<TestStatusWorker>();
                });
    }
}
