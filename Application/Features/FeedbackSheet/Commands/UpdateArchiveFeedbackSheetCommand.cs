using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class UpdateArchiveFeedbackSheetCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        public class UpdateFeedbackSheetCommandHandler : IRequestHandler<UpdateArchiveFeedbackSheetCommand, Response<int>>
        {
            private readonly ITestRepositoryAsync _testRepository;
            public UpdateFeedbackSheetCommandHandler(ITestRepositoryAsync testRepository)
            {
                _testRepository = testRepository;
            }
            public async Task<Response<int>> Handle(UpdateArchiveFeedbackSheetCommand command, CancellationToken cancellationToken)
            {
                var test = await _testRepository.GetByIdAsync(command.Id);

                if (test == null)
                {
                    throw new ApiException($"Feedback Sheet Not Found.");
                }
                else
                {
                    test.IsArchived = true;
                    await _testRepository.UpdateAsync(test);
                    return new Response<int>(test.Id);
                }
            }
        }

    }
}
