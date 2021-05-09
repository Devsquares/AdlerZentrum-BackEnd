using Application.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Features
{
    public class Downgrader
    {
        private DbContext dbContext;
        private ApplicationUser user;
        private GroupInstanceStudents currentGroup;
        private GroupInstanceStudents lastGroup;
        private StudentInfo studentInfo;

        private bool completeExecution;

        public Downgrader(DbContext dbContext, ApplicationUser user)
        {
            this.dbContext = dbContext;
            this.user = user;
            this.completeExecution = true;
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
            //Get the current group
            currentGroup = dbContext.Set<GroupInstanceStudents>()
                .Include(x => x.GroupInstance.GroupDefinition.Sublevel.Level)
                .Where(x => x.StudentId == user.Id
                && x.IsDefault == true
                && x.GroupInstance.Status == (int)GroupInstanceStatusEnum.Running)
                .FirstOrDefault();


            //Get the last group
            lastGroup = dbContext.Set<GroupInstanceStudents>()
                .Include(x => x.GroupInstance.GroupDefinition.Sublevel.Level)
                .Where(x => x.StudentId == user.Id
                && x.IsDefault == false
                && x.GroupInstance.Status == (int)GroupInstanceStatusEnum.Finished)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            studentInfo = dbContext.Set<StudentInfo>()
                .Where(x => x.StudentId == user.Id).FirstOrDefault();
        }

        private void DoExecutionChecks()
        {
            //check that there is no current group
            if (currentGroup != null)
            {
                completeExecution = false;
            }
        }

        private void Execute()
        {
            if (!completeExecution)
                return;
            //check the data of the last group and if more than 2 months ago then downgrade
            //set the new sublevel id.

            if (lastGroup.GroupInstance.GroupDefinition.EndDate.AddMonths(2) > DateTime.Now)
            {

                var currentSublevel = dbContext.Set<Sublevel>().Where(x => x.Id == studentInfo.SublevelId).FirstOrDefault();
                var preSublevel = dbContext.Set<Sublevel>().Where(x => x.Order == (currentSublevel.Order - 1)).FirstOrDefault();
                studentInfo.SublevelId = preSublevel.Id;
            }
            dbContext.SaveChanges();
        }
    }
}
