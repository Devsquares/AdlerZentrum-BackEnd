using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Process
{
    public class TestManagmentWorker : BackgroundService
    {
        private readonly ILogger<TestManagmentWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TestManagmentWorker(ILogger<TestManagmentWorker> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan interval = new TimeSpan(0, 0, 10, 0);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // Solved to corrected.
                    var testInstances = dbContext.TestInstances.Where(x => x.Status == (int)TestInstanceEnum.Solved).ToList();
                    if (testInstances != null && testInstances.Count > 0)
                    {
                        foreach (var item in testInstances)
                        {
                            var SingleQuestionSubmissions = dbContext.SingleQuestionSubmissions.Where(x => x.TestInstanceId == item.Id).ToList();
                            if (SingleQuestionSubmissions != null && SingleQuestionSubmissions.Count != 0)
                            {
                                bool corrected = false;

                                corrected = SingleQuestionSubmissions.All(x => x.Corrected == true);
                                if (corrected)
                                {
                                    item.Status = (int)TestInstanceEnum.Corrected;
                                }
                            }
                        }
                        dbContext.TestInstances.UpdateRange(testInstances);
                        dbContext.SaveChanges();
                    }

                }

                await Task.Delay(1 * 24 * 60 * 60 * 1000, stoppingToken);
            }

        }

    }
}