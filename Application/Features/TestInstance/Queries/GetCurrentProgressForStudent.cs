using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TestInstance.Queries
{
    public class GetCurrentProgressForStudent : IRequest<Response<CurrentProgressModel>>
    {
        public string StudentId { get; set; }
        public class GetCurrentProgressForStudentHandler : IRequestHandler<GetCurrentProgressForStudent, Response<CurrentProgressModel>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly IMapper _mapper;
            public GetCurrentProgressForStudentHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper,
                IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync)
            {
                _testinstanceRepository = testinstanceRepository;
                _mapper = mapper;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
            }
            public async Task<Response<CurrentProgressModel>> Handle(GetCurrentProgressForStudent query, CancellationToken cancellationToken)
            {

                
                var groupinstanceStudent = _groupInstanceStudentRepositoryAsync.GetgroupInstanceByStudentId(query.StudentId);
                if(groupinstanceStudent== null)
                {
                    throw new ApiException("The student not in groupInstance");
                }
                var GIS = groupinstanceStudent[0];
                if (groupinstanceStudent.Count > 1)
                {
                    GIS = groupinstanceStudent.LastOrDefault();// final
                }
     
               var groupInstanceIds = groupinstanceStudent.Select(x => x.GroupInstanceId).ToList();
                var testinstanceList = _testinstanceRepository.GetProgressByStudentId(query.StudentId, groupInstanceIds).Result;
                CurrentProgressModel currentProgressModel = new CurrentProgressModel();
                currentProgressModel.Quizzes.QuizInstances = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.quizz).ToList();
                foreach (var quiz in currentProgressModel.Quizzes.QuizInstances)
                {
                    currentProgressModel.Quizzes.TotalScore += quiz.Test.TotalPoint;
                    currentProgressModel.Quizzes.AchievedScore += quiz.Points;
                }

                //TODO : Remove all percent calculations and get the achievedscore from the groupinstancestudent table
                var quizPercent = 0.0;
                var studentquizPercent = 0.0;
                if (currentProgressModel.Quizzes.QuizInstances.Count > 0)
                {
                     quizPercent = GIS.GroupInstance.GroupDefinition.Sublevel.Quizpercent;
                     studentquizPercent = quizPercent * (currentProgressModel.Quizzes.AchievedScore / currentProgressModel.Quizzes.TotalScore);
                    studentquizPercent = Math.Round(studentquizPercent, 2);
                }
                currentProgressModel.Sublevels.SubLevelTests = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.subLevel).ToList();
                foreach (var sublevel in currentProgressModel.Sublevels.SubLevelTests)
                {
                    currentProgressModel.Sublevels.TotalScore += sublevel.Test.TotalPoint;
                    currentProgressModel.Sublevels.AchievedScore += sublevel.Points;
                }
                var SublevelPercent = 0.0;
                var studentSublevelPercent = 0.0;
                if (currentProgressModel.Sublevels.SubLevelTests.Count > 0)
                {
                     SublevelPercent = GIS.GroupInstance.GroupDefinition.Sublevel.SublevelTestpercent;
                     studentSublevelPercent = SublevelPercent * (currentProgressModel.Sublevels.AchievedScore / currentProgressModel.Sublevels.TotalScore);
                    studentSublevelPercent = Math.Round(studentSublevelPercent, 2);
                }
                currentProgressModel.Final.FinalTestInstances = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.final).ToList();
                foreach (var final in currentProgressModel.Final.FinalTestInstances)
                {
                    currentProgressModel.Final.TotalScore += final.Test.TotalPoint;
                    currentProgressModel.Final.AchievedScore += final.Points;
                }
                var FinalPercent = 0.0;
                var studentFinalPercent = 0.0;
                if (currentProgressModel.Final.FinalTestInstances.Count > 0)
                {
                     FinalPercent = GIS.GroupInstance.GroupDefinition.Sublevel.FinalTestpercent;
                     studentFinalPercent = FinalPercent * (currentProgressModel.Final.AchievedScore / currentProgressModel.Final.TotalScore);
                    studentFinalPercent = Math.Round(studentFinalPercent, 2);
                }
                currentProgressModel.TotalScore = 100;
                currentProgressModel.AchievedScore = studentquizPercent + studentSublevelPercent + studentFinalPercent;
                if(groupinstanceStudent.Count == 1)
                {
                    groupinstanceStudent[0].AchievedScore = currentProgressModel.AchievedScore;
                    await _groupInstanceStudentRepositoryAsync.UpdateAsync(groupinstanceStudent[0]);
                }
                else if(groupinstanceStudent.Count > 1)
                {
                    var groupstudent = groupinstanceStudent.LastOrDefault();
                    groupstudent.AchievedScore = currentProgressModel.AchievedScore;
                    await _groupInstanceStudentRepositoryAsync.UpdateAsync(groupstudent);
                }
                //currentProgressModel.TotalScore = currentProgressModel.Quizzes.TotalScore + currentProgressModel.Sublevels.TotalScore + currentProgressModel.Final.TotalScore;
                //currentProgressModel.AchievedScore = currentProgressModel.Quizzes.AchievedScore + currentProgressModel.Sublevels.AchievedScore + currentProgressModel.Final.AchievedScore;

                //var testinstanceList = _testinstanceRepository.GetProgressByStudentId(query.StudentId).Result;
                //CurrentProgressModel currentProgressModel = new CurrentProgressModel();
                //currentProgressModel.Quizzes.QuizInstances = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.quizz).ToList();
                //foreach (var quiz in currentProgressModel.Quizzes.QuizInstances)
                //{
                //    currentProgressModel.Quizzes.TotalScore += quiz.Test.TotalPoint;
                //    currentProgressModel.Quizzes.AchievedScore += quiz.Points;
                //}
                //currentProgressModel.Sublevels.SubLevelTests = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.subLevel).ToList();
                //foreach (var sublevel in currentProgressModel.Sublevels.SubLevelTests)
                //{
                //    currentProgressModel.Sublevels.TotalScore += sublevel.Test.TotalPoint;
                //    currentProgressModel.Sublevels.AchievedScore += sublevel.Points;
                //}
                //currentProgressModel.Final.FinalTestInstances = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.final).ToList();
                //foreach (var final in currentProgressModel.Final.FinalTestInstances)
                //{
                //    currentProgressModel.Final.TotalScore += final.Test.TotalPoint;
                //    currentProgressModel.Final.AchievedScore += final.Points;
                //}
                //currentProgressModel.TotalScore = currentProgressModel.Quizzes.TotalScore + currentProgressModel.Sublevels.TotalScore + currentProgressModel.Final.TotalScore;
                //currentProgressModel.AchievedScore = currentProgressModel.Quizzes.AchievedScore + currentProgressModel.Sublevels.AchievedScore + currentProgressModel.Final.AchievedScore;
                ////foreach (var testInstance in testinstanceList)
                ////{

                ////}
                //// var testinstanceViewModel = _mapper.Map<IReadOnlyList<AllTestsToManageViewModel>>(testinstance);
                return new Response<CurrentProgressModel>(currentProgressModel);
            }
        }
    }
}
