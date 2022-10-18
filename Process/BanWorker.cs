using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;
using Application.Interfaces; 
using Infrastructure.Persistence.Contexts; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Process
{
    public class BanWorker : BackgroundService
    {
        private readonly ILogger<BanWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Thread _doJob;

        public BanWorker(ILogger<BanWorker> logger,
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
                TimeSpan interval = new TimeSpan(1, 0, 0, 0);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var testInstances = dbContext.TestInstances
                                        .Where(x => x.Status == (int)TestInstanceEnum.Missed
                                        && x.Test.TestTypeId == (int)TestTypeEnum.placement).GroupBy(x => x.StudentId)
                                        .Select(x => new { studentId = x.Key, count = x.Count() }).ToList();

                    foreach (var item in testInstances)
                    {
                        if (item.count >= 3)
                        {
                            var student = dbContext.ApplicationUsers.Where(x => x.Id == item.studentId).FirstOrDefault();
                            student.BanComment = "Missed three placement tests";
                            student.Banned = true;
                            dbContext.ApplicationUsers.Update(student);
                        }
                    }
                    dbContext.SaveChanges(); 
                }
                await Task.Delay(interval, stoppingToken);
            }

        }

    }
}