using Application.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Persistence.Helpers.Calculation
{
    public class ScoreCalculator
    {

        private DbContext dbContext;
        private ApplicationUser user;

        private GroupInstanceStudents currentGroup;
        private List<GroupInstanceStudents> previousGroups;
        private List<TestInstance> quizzes;
        private List<TestInstance> sublevelTests;

        private bool isFinal;
        private TestInstance finalTest;

        private Dictionary<TestTypeEnum, int> grading;
        private int achievedScore;


        public ScoreCalculator(DbContext dbContext, ApplicationUser user)
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

            updateGrading();
        }

        private void DoInitializationChecks()
        {
        }


        /*
         * Initalizes the variables needed in the execution
         */
        private void Initalize()
        {
            //Get the current group
            currentGroup = dbContext.Set<GroupInstanceStudents>()
                .Include(x => x.GroupInstance.GroupDefinition.Sublevel.Level)
                .Where(x => x.StudentId == user.Id 
                && x.IsDefault == true 
                && x.GroupInstance.Status == (int)GroupInstanceStatusEnum.Running)
                .FirstOrDefault();

            isFinal = currentGroup.GroupInstance.GroupDefinition.Sublevel.IsFinal;

            //Get the previos groups
            previousGroups = dbContext.Set<GroupInstanceStudents>()
                .Include(x => x.GroupInstance.GroupDefinition.Sublevel.Level)
                .Where(x => x.StudentId == user.Id
                && x.IsDefault == false 
                && x.GroupInstance.Status == (int)GroupInstanceStatusEnum.Finished
                && x.GroupInstance.GroupDefinition.Sublevel.Level.Id == currentGroup.GroupInstance.GroupDefinition.Sublevel.Level.Id
                && x.Succeeded)
                .ToList();

            //get the quizzes
            quizzes = dbContext.Set<TestInstance>().Include(x => x.Test)
                .Where(x => x.Test.TestTypeId == (int)TestTypeEnum.quizz
                && x.GroupInstanceId == currentGroup.GroupInstanceId
                || previousGroups.Select(x => x.GroupInstanceId).ToList().Contains((int)x.GroupInstanceId))
                .ToList();

            //get the sublevelTests
            sublevelTests = dbContext.Set<TestInstance>().Include(x => x.Test)
                .Where(x => x.Test.TestTypeId == (int)TestTypeEnum.subLevel
                && x.GroupInstanceId == currentGroup.GroupInstanceId
                || previousGroups.Select(x => x.GroupInstanceId).ToList().Contains((int)x.GroupInstanceId))
                .ToList();

            //get the finalTest
            if (isFinal)
                finalTest = dbContext.Set<TestInstance>().Include(x => x.Test)
                    .Where(x => x.Test.TestTypeId == (int)TestTypeEnum.final
                    && x.GroupInstanceId == currentGroup.GroupInstanceId)
                    .FirstOrDefault();
            else
                finalTest = null;

            //get the grading factors
            grading.Add(TestTypeEnum.quizz, currentGroup.GroupInstance.GroupDefinition.Sublevel.Quizpercent);
            grading.Add(TestTypeEnum.subLevel, currentGroup.GroupInstance.GroupDefinition.Sublevel.SublevelTestpercent);
            grading.Add(TestTypeEnum.final, currentGroup.GroupInstance.GroupDefinition.Sublevel.FinalTestpercent);

        }

        private void DoExecutionChecks()
        {
        }

        private void Execute()
        {
            var quizAchievedScore = 0;
            var quizTotalScore = 0;

            var sublevelTestAchievedScore = 0;
            var sublevelTestTotalScore = 0;

            var finalTestAchievedScore = 0;
            var finalTestTotalScore = 0;

            var score = 0;

            //calculate quizzes
            foreach (var quiz in quizzes)
            {
                quizAchievedScore += quiz.Test.TotalPoint;
                quizTotalScore += quiz.Points;
            }

            //calculate sublevelTests 
            foreach (var sublevelTest in sublevelTests)
            {
                sublevelTestAchievedScore += sublevelTest.Test.TotalPoint;
                sublevelTestTotalScore += sublevelTest.Points;
            }

            //calculate finalTest
            if (finalTest != null)
            {
                finalTestAchievedScore += finalTest.Test.TotalPoint;
                finalTestTotalScore += finalTest.Points;
            }
    
            if (quizTotalScore != 0)
                score += grading.GetValueOrDefault(TestTypeEnum.quizz) * (quizAchievedScore / quizTotalScore);

            if (sublevelTestTotalScore != 0)
                score += grading.GetValueOrDefault(TestTypeEnum.subLevel) * (sublevelTestAchievedScore / quizTotalScore);
            
            if (finalTestTotalScore != 0)
                score += grading.GetValueOrDefault(TestTypeEnum.subLevel) * (sublevelTestAchievedScore / quizTotalScore);

            achievedScore = score;
        }

        private void updateGrading()
        {
            currentGroup.AchievedScore = achievedScore;
            dbContext.Update(currentGroup);

            // only if the sublevel is final, the upgrading/ downgrading is dependent of the grading
            if (isFinal && finalTest != null && finalTest.Status == (int) TestInstanceEnum.Corrected)
            {
                Upgrader upgrader = new Upgrader(dbContext, user);
                upgrader.CheckAndProcess();
            }
        }


        /*
         * 1 When to set the status of a group instance to be finished
         * 2 All score/points should be double instead of int
         * 3 isEligible, succeeded, nextSublevel Fields
         * 4 when the isDefault is used
         * 5 when the sublevelId in the application user application us updated
         * 6 how to control the status of the disqualified student (?) -> disqualified in the groupInstanceStudents?
         */
    }
}
