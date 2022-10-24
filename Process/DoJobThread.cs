using System;
using System.Linq;
using Application.Enums;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Application.Features;
using Microsoft.Extensions.Logging;

namespace Process
{
    public class DoJobThread
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<JobsWorker> _logger;

        public DoJobThread(IServiceProvider serviceProvider, ILogger<JobsWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public void Run(object job)
        {
            Job _job = (Job)job;
            JobTypeEnum jobType = (JobTypeEnum)_job.Type;
            _logger.LogInformation($"New Job run with id {_job.Id} and type {jobType.ToString()}");
            using (var scope = _serviceProvider.CreateScope())
            {
                // update job to running.
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                _job.Status = (int)JobStatusEnum.Running;
                _job.ExecutionDate = DateTime.Now;
                dbContext.Update(_job);
                dbContext.SaveChanges();

                switch (jobType)
                {
                    case JobTypeEnum.TestCorrection:
                        try
                        {
                            AutoCorrection autoCorrection = new AutoCorrection();
                            autoCorrection.Run(dbContext, _job.TestInstanceId.Value);
                            _job.Status = (int)JobStatusEnum.Done;
                            _job.FinishDate = DateTime.Now;
                        }
                        catch (System.Exception ex)
                        {
                            _logger.LogError($"Error in Do Job ${ex.Message}");
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;
                    case JobTypeEnum.ScoreCalculator:
                        try
                        {
                            var student = dbContext.ApplicationUsers.Where(x => x.Id == _job.StudentId).FirstOrDefault();
                            ScoreCalculator ScoreCalculator = new ScoreCalculator(dbContext, student);
                            ScoreCalculator.CheckAndProcess();
                            _job.Status = (int)JobStatusEnum.Done;
                            _job.FinishDate = DateTime.Now;
                        }
                        catch (System.Exception ex)
                        {
                            _logger.LogError($"Error in Do Job ${ex.Message}");
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;
                    case JobTypeEnum.ScoreCalculatorForGroup:
                        try
                        {
                            var studentList = dbContext.GroupInstanceStudents.Where(x => x.GroupInstanceId == _job.GroupInstanceId).ToList();
                            foreach (var groupInstanceStudent in studentList)
                            {
                                var student = dbContext.ApplicationUsers.Where(x => x.Id == groupInstanceStudent.StudentId).FirstOrDefault();
                                ScoreCalculator ScoreCalculator = new ScoreCalculator(dbContext, student);
                                ScoreCalculator.CheckAndProcess();
                            }
                            _job.Status = (int)JobStatusEnum.Done;
                            _job.FinishDate = DateTime.Now;
                        }
                        catch (System.Exception ex)
                        {
                            _logger.LogError($"Error in Do Job ${ex.Message}");
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;
                    case JobTypeEnum.Upgrader:
                        try
                        {
                            var student = dbContext.ApplicationUsers.Where(x => x.Id == _job.StudentId).FirstOrDefault();
                            Upgrader upgrader = new Upgrader(dbContext, student);
                            upgrader.CheckAndProcess();
                            _job.Status = (int)JobStatusEnum.Done;
                            _job.FinishDate = DateTime.Now;
                        }
                        catch (System.Exception ex)
                        {
                            _logger.LogError($"Error in Do Job ${ex.Message}");
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;
                    case JobTypeEnum.Downgrader:
                        try
                        {
                            var student = dbContext.ApplicationUsers.Where(x => x.Id == _job.StudentId).FirstOrDefault();
                            Downgrader downgrader = new Downgrader(dbContext, student);
                            _job.Status = (int)JobStatusEnum.Done;
                            _job.FinishDate = DateTime.Now;
                        }
                        catch (System.Exception ex)
                        {
                            _logger.LogError($"Error in Do Job ${ex.Message}");
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;

                    case JobTypeEnum.GroupFinish:
                        try
                        {
                            FinishedGroup finishedGroup = new FinishedGroup(dbContext, _job.GroupInstanceId.Value);
                            finishedGroup.CheckAndProcess();
                            _job.Status = (int)JobStatusEnum.Done;
                            _job.FinishDate = DateTime.Now;
                        }
                        catch (System.Exception ex)
                        {
                            _logger.LogError($"Error in Do Job ${ex.Message}");
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;
                    case JobTypeEnum.Disqualifier:
                        try
                        {
                            var student = dbContext.ApplicationUsers.Where(x => x.Id == _job.StudentId).FirstOrDefault();
                            Disqualifier disqualifier = new Disqualifier(dbContext, student, _job.GroupInstanceId.Value);
                            disqualifier.CheckAndProcess();
                            _job.Status = (int)JobStatusEnum.Done;
                            _job.FinishDate = DateTime.Now;
                        }
                        catch (System.Exception ex)
                        {
                            _logger.LogError($"Error in Do Job ${ex.Message}");
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                        }
                        break;
                    default:
                        break;
                }
                dbContext.Update(_job);
                dbContext.SaveChanges();

                _logger.LogInformation($"Job finished with id {_job.Id} and type {jobType.ToString()}");
            }
        }


        public static DoJobThread Create(IServiceProvider serviceProvider, ILogger<JobsWorker> logger)
        {
            return new DoJobThread(serviceProvider, logger);
        }
    }
}