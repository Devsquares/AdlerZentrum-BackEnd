using Application.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Persistence.Helpers.Calculation
{
    public class Upgrader
    {

        private static int SUCCESS_FINAL_CRITERIA = 60;
        private static int SUCCESS_ATTENDANCE_CRITERIA = 75;


        private DbContext dbContext;
        private ApplicationUser user;
        private StudentInfo studentInfo;
        private GroupInstanceStudents currentGroup;

        private bool isFinal;

        public Upgrader(DbContext dbContext, ApplicationUser user)
        {
            this.dbContext = dbContext;
            this.user = user;
            studentInfo = new StudentInfo();
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

            //checks whether the student is in a final sublevel
            isFinal = currentGroup.GroupInstance.GroupDefinition.Sublevel.IsFinal;
        }

        private void DoExecutionChecks()
        {
        }

        private void Execute()
        {

            if (isFinal)
                CheckAndExecuteFinal();
            else
                CheckAndExecuteNotFinal();
            dbContext.SaveChanges();
        }

        private void CheckAndExecuteFinal()
        {

            //get the final test
            var finalTest = dbContext.Set<TestInstance>().Include(x => x.Test)
                    .Where(x => x.Test.TestTypeId == (int)TestTypeEnum.final
                    && x.GroupInstanceId == currentGroup.GroupInstanceId)
                    .FirstOrDefault();

            // if the final test is already corrected, that means that the decision can be done
            if (finalTest != null && finalTest.Status == (int)TestInstanceEnum.Corrected)
            {
                ExecuteFinal();
            }

        }

        private void ExecuteFinal()
        {
            if (currentGroup.AchievedScore >= Upgrader.SUCCESS_FINAL_CRITERIA)
            {
                //success
                success();
            }
            else
            {
                //failed
                fail();
            }
        }

        private void CheckAndExecuteNotFinal()
        {
            var noOfLessons = currentGroup.GroupInstance.GroupDefinition.Sublevel.NumberOflessons;

            var countAttendance = dbContext.Set<LessonInstanceStudent>()
                .Include(x => x.LessonInstance.GroupInstance.GroupDefinition.Sublevel)
                .Where(x => x.StudentId == user.Id
                && x.LessonInstance.GroupInstanceId == currentGroup.Id
                && x.Attend)
                .Count();


            if ((countAttendance / noOfLessons) * 100 >= Upgrader.SUCCESS_ATTENDANCE_CRITERIA)
            {
                success();
            }
            else
            {
                var countAbsence = dbContext.Set<LessonInstanceStudent>()
                .Include(x => x.LessonInstance.GroupInstance.GroupDefinition.Sublevel)
                .Where(x => x.StudentId == user.Id
                && x.LessonInstance.GroupInstanceId == currentGroup.Id
                && x.LessonInstance.SubmittedReport
                && !x.Attend)
                .Count();

                if ((countAbsence / noOfLessons) * 100 > 100 - Upgrader.SUCCESS_ATTENDANCE_CRITERIA)
                {
                    fail();
                }
            }
        }


        public void success()
        {
            currentGroup.Succeeded = true;
            studentInfo.StudentId = user.Id;
            // TODO: change it to be with order
            studentInfo.SublevelId = currentGroup.GroupInstance.GroupDefinition.SubLevelId + 1;
            dbContext.Update(studentInfo);
            dbContext.Update(currentGroup);
            Upgrade();
        }

        public void fail()
        {
            currentGroup.Succeeded = false;
            //he should be removed from the group, do we need here a disqualified column?
            dbContext.Update(currentGroup);
        }

        private void Upgrade()
        {
            //check if the user has applied for the next sublevel (?)
            var nextGroup = dbContext.Set<GroupInstanceStudents>()
                .Include(x => x.GroupInstance.GroupDefinition.Sublevel.Level)
                .Where(x => x.StudentId == user.Id
                && x.IsDefault == false
                && x.GroupInstance.Status == (int)GroupInstanceStatusEnum.Pending
                && x.GroupInstance.GroupDefinition.Sublevel.Order == currentGroup.GroupInstance.GroupDefinition.Sublevel.Order + 1
                && !x.IsEligible)
                .FirstOrDefault();

            if (nextGroup != null)
            {
                nextGroup.IsEligible = true;
                dbContext.Update(nextGroup);
            }
        }
    }
}
