using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features
{
    public class CheckFeedbackSheetCreationQuery : IRequest<bool>
    {
        public class CheckFeedbackSheetCreationQueryHandler : IRequestHandler<CheckFeedbackSheetCreationQuery, bool>
        {
            private readonly ITestRepositoryAsync _testRepo;
            public CheckFeedbackSheetCreationQueryHandler(ITestRepositoryAsync testRepositoryAsync)
            {
                _testRepo = testRepositoryAsync;
            }
            public async Task<bool> Handle(CheckFeedbackSheetCreationQuery query, CancellationToken cancellationToken)
            {
                int feedbbackSheetCount = await _testRepo.FeedbackSheetNotArchivedCount();
                if (feedbbackSheetCount > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }
    }
}