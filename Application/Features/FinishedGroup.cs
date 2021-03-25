using Application.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Features
{
    public class FinishedGroup
    {
        private DbContext dbContext;
        private int groupInstanceId;
        private List<GroupInstanceStudents> students;
        private GroupInstance group;

        public FinishedGroup(DbContext dbContext, int groupInstanceId)
        {
            this.dbContext = dbContext;
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
            //Get the current group
            group = dbContext.Set<GroupInstance>()
                .Include(x => x.Students)
                .ThenInclude(x => x.Student)
                .Where(x => x.Id == groupInstanceId
                && x.Status == (int)GroupInstanceStatusEnum.Running)
                .FirstOrDefault();

            students = group.Students.ToList();
        }

        private void DoExecutionChecks()
        {
            //check that there is no current group
        }

        private void Execute()
        {
            List<Job> jobs = new List<Job>();
            foreach (var item in students)
            {
                item.IsDefault = false;
                jobs.Add(new Job
                {
                    Type = (int)JobTypeEnum.Downgrader,
                    StudentId = item.StudentId,
                    Status = (int)JobStatusEnum.New,
                    StartDate = DateTime.Now.AddMonths(2)
                });
            }

            group.Status = (int)GroupDefinationStatusEnum.Finished;
            dbContext.AddRange(jobs);
            dbContext.Update(group); 
            dbContext.SaveChanges();
        }
    }
}
