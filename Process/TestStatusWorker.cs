using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Process
{
    public class TestStatusWorker : BackgroundService
    {
        private readonly ILogger<TestStatusWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Thread _doJob;
        private const int HOURS = 3;

        public TestStatusWorker(ILogger<TestStatusWorker> logger,
            IServiceProvider serviceProvider,
            IEmailService emailService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan interval = new TimeSpan(0, 1, 0, 0);
                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var testInstances = dbContext.TestInstances
                                            .Where(x => x.Status == (int)TestInstanceEnum.Pending
                                            && x.StartDate.Value.AddHours(HOURS) > DateTime.Now).ToList();

                        foreach (var item in testInstances)
                        {
                            item.Status = (int)TestInstanceEnum.Missed;
                        }
                        dbContext.TestInstances.UpdateRange(testInstances);
                        dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }

                }
                await Task.Delay(interval, stoppingToken);
            }

        }

    }
}