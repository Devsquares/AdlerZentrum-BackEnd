using System.Collections.Generic;
using System.Linq;
using Application.Enums;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

public class AutoCorrection
{
    private ApplicationDbContext dbContext;
    private int testId;
    public void Run(ApplicationDbContext _dbContext, int _testId)
    {
        dbContext = _dbContext;
        testId = _testId;
    }
    private void CorrectTheTest()
    {
        var testInstance = dbContext.TestInstances
                                       .Where(x => x.Status == (int)TestInstanceEnum.Solved && x.Id == testId).FirstOrDefault();


        var singleQuestions = dbContext.SingleQuestionSubmissions.Include(x => x.SingleQuestion).ThenInclude(x => x.Choices)
        .Include(x => x.Choices)
                                       .Where(x => x.TestInstanceId == testId).ToListAsync().Result;

        bool autoCorrected = correctTheQuestions(dbContext, singleQuestions, out double points);

        testInstance.Points += points;
        // check if all single quesition not corrected, if so then add task to the teacher to correct it.
        if (autoCorrected)
        {
            testInstance.Status = (int)TestInstanceEnum.Corrected;
        }
        else
        {
            testInstance.ManualCorrection = true;
        }
        //TODO add required submtion date.
        dbContext.TestInstances.Update(testInstance);
        dbContext.SaveChanges();
    }

    private bool correctTheQuestions(ApplicationDbContext dbContext, List<SingleQuestionSubmission> SingleQuestionSubmissions, out double points)
    {
        bool autoCorrected = true;
        points = 0;
        foreach (var item in SingleQuestionSubmissions)
        {
            // TrueAndfalse
            if (item.SingleQuestion.SingleQuestionType == (int)SingleQuestionTypeEnum.TrueAndfalse)
            {
                if (item.TrueOrFalseSubmission == item.SingleQuestion.AnswerIsTrueOrFalse)
                {
                    item.RightAnswer = true;
                    item.Points = item.SingleQuestion.Points;
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
                    item.Points = item.SingleQuestion.Points;
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
                    item.RightAnswer = true;
                    foreach (var submtionchoice in item.Choices)
                    {
                        if (!correctIds.Contains(submtionchoice.Id))
                        {
                            item.RightAnswer = false;
                            break;
                        }
                    }
                    if (item.RightAnswer)
                    {
                        item.Points = item.SingleQuestion.Points;
                        points += item.SingleQuestion.Points;
                    }

                }

                if (item.Choices.Count == item.SingleQuestion.Choices.Count)
                {
                    item.RightAnswer = true;
                    item.Points = item.SingleQuestion.Points;
                    points += item.SingleQuestion.Points;
                }
                else
                {
                    item.RightAnswer = false;
                }
                item.Corrected = true;
            }
            else
            {
                autoCorrected = false;
            }
        }

        dbContext.SingleQuestionSubmissions.UpdateRange(SingleQuestionSubmissions);
        return autoCorrected;
    }

}

