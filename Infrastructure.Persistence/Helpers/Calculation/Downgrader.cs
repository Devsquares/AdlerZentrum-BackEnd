using Application.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Persistence.Helpers.Calculation
{
    class Downgrader
    {
        private DbContext dbContext;
        private ApplicationUser user;
        private GroupInstanceStudents currentGroup;
        private GroupInstanceStudents lastGroup;

        private bool completeExecution;

        public Downgrader(DbContext dbContext, ApplicationUser user)
        {
            this.dbContext = dbContext;
            this.user = user;
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

        }

        private void DoExecutionChecks()
        {
            //check that there is no current group
        }

        private void Execute()
        {
            if (!completeExecution)
                return;

            //check the data of the last group and if more than 2 months ago then downgrade
        }



    }
}
