using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application.Features;

namespace Process
{
    public class DoJobThread
    {
        private readonly IServiceProvider _serviceProvider;

        public DoJobThread(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void Run(object job)
        {
            Job _job = (Job)job;
            using (var scope = _serviceProvider.CreateScope())
            {
                // update job to running.
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                _job.Status = (int)JobStatusEnum.Running;
                _job.ExecutionDate = DateTime.Now;
                dbContext.Update(_job);
                dbContext.SaveChanges();

                switch (_job.Type)
                {
                    case (int)JobTypeEnum.TestCorrection:
                        try
                        {
                            AutoCorrection autoCorrection = new AutoCorrection();
                            autoCorrection.Run(dbContext, _job.TestInstanceId.Value);
                        }
                        catch (System.Exception ex)
                        {
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;
                    case (int)JobTypeEnum.ScoreCalculator:
                        try
                        {
                            var student = dbContext.ApplicationUsers.Where(x => x.Id == _job.StudentId).FirstOrDefault();
                            ScoreCalculator ScoreCalculator = new ScoreCalculator(dbContext, student);
                            ScoreCalculator.CheckAndProcess();
                        }
                        catch (System.Exception ex)
                        {
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;
                    case (int)JobTypeEnum.Upgrader:
                        try
                        {
                            var student = dbContext.ApplicationUsers.Where(x => x.Id == _job.StudentId).FirstOrDefault();
                            Upgrader upgrader = new Upgrader(dbContext, student);
                            upgrader.CheckAndProcess();
                        }
                        catch (System.Exception ex)
                        {
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;
                    case (int)JobTypeEnum.Downgrader:
                        try
                        {
                            var student = dbContext.ApplicationUsers.Where(x => x.Id == _job.StudentId).FirstOrDefault();
                            Downgrader downgrader = new Downgrader(dbContext, student);
                        }
                        catch (System.Exception ex)
                        {
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;

                    case (int)JobTypeEnum.GroupFinish:
                        try
                        {
                            FinishedGroup finishedGroup = new FinishedGroup(dbContext, _job.GroupInstanceId.Value);
                            finishedGroup.CheckAndProcess();
                        }
                        catch (System.Exception ex)
                        {
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;
                    case (int)JobTypeEnum.Disqualifier:
                        try
                        {
                            var student = dbContext.ApplicationUsers.Where(x => x.Id == _job.StudentId).FirstOrDefault();
                            Disqualifier disqualifier = new Disqualifier(dbContext, student, _job.GroupInstanceId.Value);
                            disqualifier.CheckAndProcess();
                        }
                        catch (System.Exception ex)
                        {
                            _job.Failure = ex.Message;
                            _job.Status = (int)JobStatusEnum.Failed;
                            dbContext.Update(_job);
                            dbContext.SaveChanges();
                        }
                        break;
                    default:
                        break;
                }
                _job.Status = (int)JobStatusEnum.Done;
                _job.FinishDate = DateTime.Now;
                dbContext.Update(_job);
                dbContext.SaveChanges();
            }
        }


        public static DoJobThread Create(IServiceProvider serviceProvider)
        {
            return new DoJobThread(serviceProvider);
        }
    }
}