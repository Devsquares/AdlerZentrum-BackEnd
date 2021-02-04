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
    public class DownGradeWorker : BackgroundService
    {
        private readonly ILogger<DownGradeWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DownGradeWorker(ILogger<DownGradeWorker> logger,
            IServiceProvider serviceProvider)
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
                    var LessonInstanceStudents = dbContext.LessonInstanceStudents
                        .Include(x => x.LessonInstance)
                        .Include(x => x.LessonInstance.GroupInstance);
                        // .Where(x => x.ReportDateTime > DateTime.Now.AddDays(14) && x.Attend == false && x.LessonInstance.GroupInstance.Status == (int)GroupInstanceStatusEnum.Running).ToList();
                    // send to him reme
                    // _logger.LogInformation("Students to be warring Count: {}", LessonInstanceStudents.Count);
                }
                // get last attend if over two weeks.
                // check if it is the active roup or not.
                // send remiener.
                // down grade if it is the 4th remeinder. 
                await Task.Delay(1 * 24 * 60 * 60 * 1000, stoppingToken);
            }

        }

    }
}