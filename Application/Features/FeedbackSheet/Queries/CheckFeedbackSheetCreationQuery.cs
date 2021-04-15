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
    public class GetFeedbackSheetQuery : IRequest<Test>
    {
        public class GetFeedbackSheetQueryHandler : IRequestHandler<GetFeedbackSheetQuery, Test>
        {
            private readonly ITestRepositoryAsync _testRepo;
            public GetFeedbackSheetQueryHandler(ITestRepositoryAsync testRepositoryAsync)
            {
                _testRepo = testRepositoryAsync;
            }
            public async Task<Test> Handle(GetFeedbackSheetQuery query, CancellationToken cancellationToken)
            {
                return await _testRepo.GetFeedbackSheet();
            }
        }
    }
}