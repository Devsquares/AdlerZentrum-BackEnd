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
    public class  UpdateTestInstanceStatusAllCommand : IRequest<Response<bool>>
    {
        public int GroupInstanceId { get; set; }
        public int TestId { get; set; }
        public int Status { get; set; }

        public class UpdateTestInstanceStatusAllCommandHandler : IRequestHandler<UpdateTestInstanceStatusAllCommand, Response<bool>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public UpdateTestInstanceStatusAllCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<bool>> Handle(UpdateTestInstanceStatusAllCommand command, CancellationToken cancellationToken)
            {
                var testInstances = await _testinstanceRepository.GetAllTestInstancesByGroupAndTest(command.GroupInstanceId, command.TestId);

                if (testInstances == null)
                {
                    throw new ApiException($"Test Instance Not Found.");
                }
                else
                {
                    foreach (var item in testInstances)
                    {
                        if (command.Status == (int)TestInstanceEnum.Pending)
                        {
                            item.Status = (int)TestInstanceEnum.Pending;
                            item.OpenDate = DateTime.Now;
                        }
                        else if (command.Status == (int)TestInstanceEnum.Missed)
                        {
                            if (item.Status != (int)TestInstanceEnum.Solved)
                            {
                                item.Status = command.Status;
                                item.SubmissionDate = DateTime.Now;
                                item.Points = 0;
                            }
                        }
                        else
                        {
                            item.Status = command.Status;
                        }

                    }

                    await _testinstanceRepository.UpdateBulkAsync(testInstances);
                    return new Response<bool>(true);
                }
            }
        }

    }
}
