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
    public class UpdateTestInstanceStatusByGroupCommand : IRequest<Response<bool>>
    {
        public int GroupInstanceId { get; set; }
        public int Status { get; set; }

        public class UpdateTestInstanceStatusByGroupCommandHandler : IRequestHandler<UpdateTestInstanceStatusByGroupCommand, Response<bool>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public UpdateTestInstanceStatusByGroupCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<bool>> Handle(UpdateTestInstanceStatusByGroupCommand command, CancellationToken cancellationToken)
            {
                var testInstances = await _testinstanceRepository.GetAllTestInstancesByGroup(command.GroupInstanceId);

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
