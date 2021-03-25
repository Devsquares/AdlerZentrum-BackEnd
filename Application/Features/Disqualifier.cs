using Application.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Features
{
    public class Disqualifier
    {
        private DbContext dbContext;
        private ApplicationUser user;
        private List<TestInstance> testInstances;
        private List<HomeWorkSubmition> homeworks;

        private List<LessonInstanceStudent> lessonInstances;
        private int groupInstanceId;

        public Disqualifier(DbContext dbContext, ApplicationUser user, int groupInstanceId)
        {
            this.dbContext = dbContext;
            this.user = user;
            this.groupInstanceId = groupInstanceId;
        }

        public void CheckAndProcess()
        {
            DoInitializationChecks();

            Initalize();

            DoExecutionChecks();

            Execute();
        }

        private void DoInitializationChecks()
        {
        }


        private void Initalize()
        {
            testInstances = dbContext.Set<TestInstance>()
                .Where(x => x.StudentId == user.Id && x.GroupInstanceId == groupInstanceId && x.Status == (int)TestInstanceEnum.Pending)
                .ToList();


            homeworks = dbContext.Set<HomeWorkSubmition>()
                .Where(x => x.StudentId == user.Id
                && x.Homework.GroupInstanceId == groupInstanceId
                && x.Status == (int)HomeWorkSubmitionStatusEnum.Pending)
                .ToList();

            lessonInstances = dbContext.Set<LessonInstanceStudent>()
            .Include(x => x.LessonInstance.GroupInstance)
            .Where(x => x.StudentId == user.Id && x.LessonInstance.GroupInstanceId == groupInstanceId)
            .ToList();
        }

        private void DoExecutionChecks()
        {
            //check that there is no current group
        }

        private void Execute()
        {
            foreach (var item in testInstances)
            {
                item.Status = (int)TestInstanceEnum.Canceled;
            }

            foreach (var item in homeworks)
            {
                item.Status = (int)HomeWorkSubmitionStatusEnum.Canceled;
            }

            foreach (var item in lessonInstances)
            {
                item.Disqualified = true;
            }

            dbContext.UpdateRange(testInstances);
            dbContext.UpdateRange(homeworks);
            dbContext.UpdateRange(lessonInstances);
            dbContext.SaveChanges();
        }
    }
}
