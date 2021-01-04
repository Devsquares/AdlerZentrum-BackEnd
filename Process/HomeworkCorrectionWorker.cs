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
    public class HomeworkCorrectionWorker : BackgroundService
    {
        private readonly ILogger<HomeworkCorrectionWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public HomeworkCorrectionWorker(ILogger<HomeworkCorrectionWorker> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


                    var homeWorkSubmitions = dbContext.TestInstances
                                            .Include(x => x.Test)
                                            .Include(x => x.Test.Questions)
                                            .ThenInclude(x => x.SingleQuestionSubmissions)
                                            .ThenInclude(x => x.Choices)
                                            .ThenInclude(x => x.Choice)
                                            .Include(x => x.Test.Questions)
                                            .ThenInclude(x => x.SingleQuestionSubmissions)
                                            .ThenInclude(x => x.SingleQuestion)
                                            .Where(x => x.Status == (int)TestInstanceEnum.Solved).ToListAsync().Result;
                    _logger.LogInformation("HomeWork Submitions to be correct Count: {}", homeWorkSubmitions.Count);


                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
