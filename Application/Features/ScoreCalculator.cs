using Application.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Features
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
        private bool isFinishedGroup;

        private Dictionary<TestTypeEnum, double> grading;
        private double achievedScore;
        private double homeworkBouns = 0.0;


        public ScoreCalculator(DbContext dbContext, ApplicationUser user)
        {
            this.dbContext = dbContext;
            this.user = user;
            this.isFinishedGroup = false;
        }
        public void CheckAndProcess()
        {
            DoInitializationChecks();

            Initalize();

            DoExecutionChecks();

            Execute();

            updateGrading();

            finishedGroup();
        }


        private void DoInitializationChecks()
        {
        }


        /*
         * Initalizes the variables needed in the execution
         */
        private void Initalize()
        {
            //Get the current group for example A1.3
            currentGroup = dbContext.Set<GroupInstanceStudents>()
                .Include(x => x.GroupInstance.GroupDefinition.Sublevel.Level)
                .Where(x => x.StudentId == user.Id
                && x.IsDefault == true
                && x.GroupInstance.Status == (int)GroupInstanceStatusEnum.Running)
                .FirstOrDefault();

            isFinal = currentGroup.GroupInstance.GroupDefinition.Sublevel.IsFinal;

            //Get the previos groups for example A1.1,A1.2
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

            isFinishedGroup = dbContext.Set<TestInstance>().Where(x => x.GroupInstanceId == currentGroup.GroupInstanceId)
            .AllAsync(x => x.Status == (int)TestInstanceEnum.Corrected || x.Status == (int)TestInstanceEnum.Missed).Result;

            homeworkBouns = dbContext.Set<HomeWorkSubmition>()
            .Include(x => x.Homework)
            .Where(x => x.StudentId == user.Id && x.Homework.GroupInstanceId == currentGroup.Id)
            .Sum(x => x.BonusPoints);

        }

        private void DoExecutionChecks()
        {
        }

        private void Execute()
        {
            var quizAchievedScore = 0.0;
            var quizTotalScore = 0.0;

            var sublevelTestAchievedScore = 0.0;
            var sublevelTestTotalScore = 0.0;

            var finalTestAchievedScore = 0.0;
            var finalTestTotalScore = 0.0;

            var score = 0.0;

            //calculate quizzes
            foreach (var quiz in quizzes)
            {
                quizAchievedScore += quiz.Points;
                quizTotalScore += quiz.Test.TotalPoint;
            }
            quizAchievedScore += homeworkBouns; 

            //calculate sublevelTests 
            foreach (var sublevelTest in sublevelTests)
            {
                sublevelTestAchievedScore += sublevelTest.Points;
                sublevelTestTotalScore += sublevelTest.Test.TotalPoint;
            }

            //calculate finalTest
            if (finalTest != null)
            {
                finalTestAchievedScore += finalTest.Points;
                finalTestTotalScore += finalTest.Test.TotalPoint;
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
            if (isFinal && finalTest != null && finalTest.Status == (int)TestInstanceEnum.Corrected)
            {
                Upgrader upgrader = new Upgrader(dbContext, user);
                upgrader.CheckAndProcess();
            }

            dbContext.SaveChanges();
        }

        private void finishedGroup()
        {
            if (isFinishedGroup)
            {
                var job = new Job
                {
                    Type = (int)JobTypeEnum.GroupFinish,
                    GroupInstanceId = currentGroup.GroupInstanceId,
                    Status = (int)JobStatusEnum.New
                };
                dbContext.Add(job);
            }
        }
    }
}
