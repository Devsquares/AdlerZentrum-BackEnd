using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums; 
using Infrastructure.Persistence.Contexts; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Process
{
    public class JobsWorker : BackgroundService
    {
        private readonly ILogger<JobsWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Thread _doJob;
        private DoJobThread _doJobThread;

        public JobsWorker(ILogger<JobsWorker> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _doJobThread = DoJobThread.Create(_serviceProvider, _logger);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            { 
                TimeSpan interval = new TimeSpan(0, 0, 0, 10);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var Jobs = dbContext.Jobs
                       .Where(x => x.Status == (int)JobStatusEnum.New
                       && (x.StartDate > DateTime.Now || x.StartDate == null)
                       ).FirstOrDefault();
                    if (Jobs != null)
                    {
                        _logger.LogInformation($"Job Task excute job with id {Jobs.Id.ToString()}");
                        _doJob = new Thread(new ParameterizedThreadStart(_doJobThread.Run));
                        _doJob.Start(Jobs);
                        await Task.Delay(interval, stoppingToken);
                    }
                }

                await Task.Delay(interval, stoppingToken);
            }

        }

    }
}