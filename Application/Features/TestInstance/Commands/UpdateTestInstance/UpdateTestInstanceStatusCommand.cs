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
    public class UpdateTestInstanceStatusCommand : IRequest<Response<int>>
    {
        public int Status { get; set; }
        public int LessonInstanceId { get; set; }

        public class UpdateTestInstanceStatusCommandHandler : IRequestHandler<UpdateTestInstanceStatusCommand, Response<int>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public UpdateTestInstanceStatusCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<int>> Handle(UpdateTestInstanceStatusCommand command, CancellationToken cancellationToken)
            {
                var testinstance = await _testinstanceRepository.GetTestInstanceByLessonInstanceId(command.LessonInstanceId);

                if (testinstance == null)
                {
                    throw new ApiException($"TestInstance Not Found.");
                }
                else
                {
                    if (command.Status == (int)TestInstanceEnum.Pending)
                    {
                        foreach (var item in testinstance)
                        {
                            item.Status = command.Status;
                        }
                        await _testinstanceRepository.UpdateBulkAsync(testinstance);
                    }
                    else if (command.Status == (int)TestInstanceEnum.Closed)
                    {
                        foreach (var item in testinstance)
                        {
                            if(item.Status != (int)TestInstanceEnum.Solved)
                            item.Status = command.Status;
                        }
                        await _testinstanceRepository.UpdateBulkAsync(testinstance);

                    }
                    else
                    {
                        throw new ApiException($"Invalid Status.");
                    }

                    return new Response<int>(1);
                }
            }
        }

    }
}
