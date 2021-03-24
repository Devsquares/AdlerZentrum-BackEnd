using Application.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Persistence.Helpers
{
    public class Disqualifier
    {
        private DbContext dbContext;
        private ApplicationUser user;
        private GroupInstanceStudents currentGroup;
        private List<TestInstance> testInstances;
        private List<HomeWorkSubmition> homeWorkSubmitions;

        public Disqualifier(DbContext dbContext, ApplicationUser user)
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

            foreach (var item in homeWorkSubmitions)
            {
                item.Status = (int)HomeWorkSubmitionStatusEnum.Canceled;
            }
            currentGroup.IsDefault = false;
            
            dbContext.SaveChanges();
        }



    }
}
