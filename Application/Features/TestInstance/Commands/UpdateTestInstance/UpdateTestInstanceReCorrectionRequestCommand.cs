using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class UpdateTestInstanceReCorrectionRequestCommand : IRequest<Response<bool>>
    {
        public int TestInstanceId { get; set; }
        public bool status { get; set; }
        public class UpdateTestInstanceReCorrectionRequestCommandHandler : IRequestHandler<UpdateTestInstanceReCorrectionRequestCommand, Response<bool>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public UpdateTestInstanceReCorrectionRequestCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<bool>> Handle(UpdateTestInstanceReCorrectionRequestCommand command, CancellationToken cancellationToken)
            {
                var TestInstance = await _testinstanceRepository.GetByIdAsync(command.TestInstanceId);

                if (TestInstance == null)
                {
                    throw new ApiException($"Test Instance Not Found.");
                }
                else
                {
                    TestInstance.ReCorrectionRequest = command.status;
                    await _testinstanceRepository.UpdateAsync(TestInstance);
                    return new Response<bool>(true);
                }
            }
        }

    }
}
