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
    public class UpdateTestInstanceReopenedCommand : IRequest<Response<bool>>
    {
        public int TestInstanceId { get; set; }
        public bool status { get; set; }

        public class UpdateTestInstanceReopenedCommandHandler : IRequestHandler<UpdateTestInstanceReopenedCommand, Response<bool>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public UpdateTestInstanceReopenedCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<bool>> Handle(UpdateTestInstanceReopenedCommand command, CancellationToken cancellationToken)
            {
                var TestInstance = await _testinstanceRepository.GetByIdAsync(command.TestInstanceId);

                if (TestInstance == null)
                {
                    throw new ApiException($"Test Instance Not Found.");
                }
                if (TestInstance.Status != (int)TestInstanceEnum.Corrected)
                {
                    throw new ApiException($"The Test status isn't corrected ");
                }
                else
                {
                    TestInstance.Reopened = command.status;
                    if(!command.status)
                    {
                        TestInstance.ReCorrectionRequest = false;
                    }
                    await _testinstanceRepository.UpdateAsync(TestInstance);
                    return new Response<bool>(true);
                }
            }
        }

    }
}
