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

namespace Process
{
    public class DoJobThread
    {
        private ApplicationDbContext applicationDbContext;
        private readonly IServiceProvider _serviceProvider;

        public void Run(object job)
        {
            Job _job = (Job)job;
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                switch (_job.Type)
                {
                    case (int)JobTypeEnum.TestCorrection:

                        break;
                    default:
                        break;
                }
            }
        }

        private void CorrectTheTest(ApplicationDbContext dbContext, int testId)
        {
            var testInstance = dbContext.TestInstances
                                           .Where(x => x.Status == (int)TestInstanceEnum.Solved && x.Id == testId).FirstOrDefault();


            var singleQuestions = dbContext.SingleQuestionSubmissions
                                           .Where(x => x.TestInstanceId == testId).ToListAsync().Result;

            int points = 0;
            points = correctTheQuestions(dbContext, singleQuestions, points);

            testInstance.Points += points;
            // check if all single quesition not corrected, if so then add task to the teacher to correct it.

            testInstance.Status = (int)TestInstanceEnum.Corrected;
            dbContext.TestInstances.Update(testInstance);

        }

        private int correctTheQuestions(ApplicationDbContext dbContext, List<SingleQuestionSubmission> SingleQuestionSubmissions, int points)
        {
            foreach (var item in SingleQuestionSubmissions)
            {
                // TrueAndfalse
                if (item.SingleQuestion.SingleQuestionType == (int)SingleQuestionTypeEnum.TrueAndfalse)
                {
                    if (item.TrueOrFalseSubmission == item.SingleQuestion.AnswerIsTrueOrFalse)
                    {
                        item.RightAnswer = true;
                        points += item.SingleQuestion.Points;
                    }
                    else
                    {
                        item.RightAnswer = false;
                    }
                    item.Corrected = true;
                }

                // SingleChoice
                else if (item.SingleQuestion.SingleQuestionType == (int)SingleQuestionTypeEnum.SingleChoice)
                {
                    if (item.Choices.FirstOrDefault().ChoiceSubmissionId == item.SingleQuestion.Choices.Where(x => x.IsCorrect).FirstOrDefault().Id)
                    {
                        item.RightAnswer = true;
                        points += item.SingleQuestion.Points;
                    }
                    else
                    {
                        item.RightAnswer = false;
                    }
                    item.Corrected = true;
                }

                // MultipleChoice
                else if (item.SingleQuestion.SingleQuestionType == (int)SingleQuestionTypeEnum.MultipleChoice)
                {
                    // get all correct , then check if this list equal or not.
                    if (item.Choices.Count != item.SingleQuestion.Choices.Where(x => x.IsCorrect == true).Count())
                    {
                        item.RightAnswer = false;
                    }
                    else
                    {
                        var correctIds = item.SingleQuestion.Choices.Where(x => x.IsCorrect == true).Select(x => x.Id);
                        foreach (var submtionchoice in item.Choices)
                        {
                            if (!correctIds.Contains(submtionchoice.Id))
                            {
                                item.RightAnswer = false;
                                break;
                            }
                        }

                        item.RightAnswer = true;
                        points += item.SingleQuestion.Points;
                    }

                    if (item.Choices.Count == item.SingleQuestion.Choices.Count)
                    {
                        item.RightAnswer = true;
                        points += item.SingleQuestion.Points;
                    }
                    else
                    {
                        item.RightAnswer = false;
                    }
                    item.Corrected = true;
                }
            }

            dbContext.SingleQuestionSubmissions.UpdateRange(SingleQuestionSubmissions);
            return points;
        }

        public static DoJobThread Create()
        {
            return new DoJobThread();
        }
    }
}