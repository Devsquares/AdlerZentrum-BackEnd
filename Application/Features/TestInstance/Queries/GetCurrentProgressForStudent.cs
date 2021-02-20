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
                currentProgressModel.Quizzes.TestInstances = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.quizz).ToList();
                currentProgressModel.Sublevels.SubLevelTests = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.subLevel).ToList();
                currentProgressModel.Final.TestInstances = testinstanceList.Where(x => x.Test.TestTypeId == (int)TestTypeEnum.final).ToList();

                //foreach (var testInstance in testinstanceList)
                //{

                //}
                // var testinstanceViewModel = _mapper.Map<IReadOnlyList<AllTestsToManageViewModel>>(testinstance);
                return new Response<CurrentProgressModel>(currentProgressModel);
            }
        }
    }
}
