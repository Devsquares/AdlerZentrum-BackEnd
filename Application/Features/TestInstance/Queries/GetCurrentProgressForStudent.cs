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
            private readonly IMapper _mapper;
            public GetCurrentProgressForStudentHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
            {
                _testinstanceRepository = testinstanceRepository;
                _mapper = mapper;
            }
            public async Task<Response<CurrentProgressModel>> Handle(GetCurrentProgressForStudent query, CancellationToken cancellationToken)
            {
                var testinstanceList = _testinstanceRepository.GetProgressByStudentId(query.StudentId).Result;
                CurrentProgressModel currentProgressModel = new CurrentProgressModel();
                currentProgressModel.Quizzes.QuizInstances = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.quizz).ToList();
                foreach (var quiz in currentProgressModel.Quizzes.QuizInstances)
                {
                    currentProgressModel.Quizzes.TotalScore += quiz.Test.TotalPoint;
                    currentProgressModel.Quizzes.AchievedScore += quiz.Points;
                }
                currentProgressModel.Sublevels.SubLevelTests = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.subLevel).ToList();
                foreach (var sublevel in currentProgressModel.Sublevels.SubLevelTests)
                {
                    currentProgressModel.Sublevels.TotalScore += sublevel.Test.TotalPoint;
                    currentProgressModel.Sublevels.AchievedScore += sublevel.Points;
                }
                currentProgressModel.Final.FinalTestInstances = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.final).ToList();
                foreach (var final in currentProgressModel.Final.FinalTestInstances)
                {
                    currentProgressModel.Final.TotalScore += final.Test.TotalPoint;
                    currentProgressModel.Final.AchievedScore += final.Points;
                }
                currentProgressModel.TotalScore = currentProgressModel.Quizzes.TotalScore + currentProgressModel.Sublevels.TotalScore + currentProgressModel.Final.TotalScore;
                currentProgressModel.AchievedScore = currentProgressModel.Quizzes.AchievedScore + currentProgressModel.Sublevels.AchievedScore + currentProgressModel.Final.AchievedScore;
                //foreach (var testInstance in testinstanceList)
                //{

                //}
                // var testinstanceViewModel = _mapper.Map<IReadOnlyList<AllTestsToManageViewModel>>(testinstance);
                return new Response<CurrentProgressModel>(currentProgressModel);
            }
        }
    }
}
