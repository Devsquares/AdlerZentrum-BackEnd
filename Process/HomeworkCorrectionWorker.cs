using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Process
{
    public class HomeworkCorrectionWorker : BackgroundService
    {
        private readonly ILogger<HomeworkCorrectionWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public HomeworkCorrectionWorker(ILogger<HomeworkCorrectionWorker> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan interval = new TimeSpan(1, 0, 0);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


                    var testInstances = dbContext.TestInstances
                                            .Include(x => x.Test)
                                            .Include(x => x.Test.Questions)
                                            .ThenInclude(x => x.SingleQuestionSubmissions)
                                            .ThenInclude(x => x.Choices)
                                            .Include(x => x.Test.Questions)
                                            .ThenInclude(x => x.SingleQuestionSubmissions)
                                            .ThenInclude(x => x.SingleQuestion)
                                            .Where(x => x.Status == (int)TestInstanceEnum.Solved).ToListAsync().Result;

                    correctTheTests(dbContext, testInstances);

                    _logger.LogInformation("HomeWork Submitions to be correct Count: {}", testInstances.Count);

                }
                await Task.Delay(interval.Milliseconds, stoppingToken);
            }

            static void correctTheTests(ApplicationDbContext dbContext, System.Collections.Generic.List<Domain.Entities.TestInstance> testInstances)
            {
                foreach (var testInstance in testInstances)
                {
                    int points = 0;
                    points = correctTheQuestions(dbContext, testInstance, points);

                    testInstance.Points = points;
                    if (testInstance.Test.AutoCorrect == true)
                    {
                        testInstance.Status = (int)TestInstanceEnum.Corrected;
                        dbContext.TestInstances.Update(testInstance);
                    }
                }
            }
        }

        private static int correctTheQuestions(ApplicationDbContext dbContext, Domain.Entities.TestInstance testInstance, int points)
        {
            foreach (var Question in testInstance.Test.Questions)
            {
                if (Question.QuestionTypeId == (int)QuestionTypeEnum.GroupOfSingle)
                {
                    foreach (var item in Question.SingleQuestionSubmissions)
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
                            dbContext.SingleQuestionSubmissions.Update(item);
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
                            dbContext.SingleQuestionSubmissions.Update(item);
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
                            dbContext.SingleQuestionSubmissions.Update(item);
                        }

                    }
                }
            }

            return points;
        }
    }
}