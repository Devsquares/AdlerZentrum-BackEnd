using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features
{
    public class GetFeedbackSheetInstancesForStudentByGroupInstanceIdQuery : IRequest<IReadOnlyList<TestInstance>>
    {
        public string StudentId { get; set; }
        public int? GroupInstanceId { get; set; }
        public class GetFeedbackSheetInstancesForStudentByGroupInstanceIdQueryHandler : IRequestHandler<GetFeedbackSheetInstancesForStudentByGroupInstanceIdQuery, IReadOnlyList<TestInstance>>
        {
            private readonly ITestInstanceRepositoryAsync _testInstanceRepository;
            public GetFeedbackSheetInstancesForStudentByGroupInstanceIdQueryHandler(ITestInstanceRepositoryAsync testRepositoryAsync)
            {
                _testInstanceRepository = testRepositoryAsync;
            }
            public async Task<IReadOnlyList<TestInstance>> Handle(GetFeedbackSheetInstancesForStudentByGroupInstanceIdQuery query, CancellationToken cancellationToken)
            {
                if (query.GroupInstanceId == null)
                {
                    return await _testInstanceRepository.GetFeedbackSheetInstancesForStudent(query.StudentId);
                }
                else
                {
                    return await _testInstanceRepository.GetFeedbackSheetInstancesForStudentByGroupInstanceId(query.StudentId, query.GroupInstanceId.Value);
                }
            }
        }
    }
}