using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Process
{
    public class MailWorker : BackgroundService
    {
        private readonly ILogger<MailWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Thread _doJob;
        private SendMailThread _doJobThread;

        public MailWorker(ILogger<MailWorker> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _doJobThread = SendMailThread.Create(_serviceProvider, logger);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan interval = new TimeSpan(0, 0, 0, 10);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var Jobs = dbContext.MailJobs
                       .Where(x => x.Status == (int)JobStatusEnum.New
                       && (x.StartDate > DateTime.Now || x.StartDate == null)
                       ).ToList();
                    if (Jobs.Count > 0)
                    {
                        _doJob = new Thread(new ParameterizedThreadStart(_doJobThread.Run));
                        _doJob.Start(Jobs[0]);
                        await Task.Delay(interval, stoppingToken);
                    }
                }

                await Task.Delay(interval, stoppingToken);
            }

        }

    }
}