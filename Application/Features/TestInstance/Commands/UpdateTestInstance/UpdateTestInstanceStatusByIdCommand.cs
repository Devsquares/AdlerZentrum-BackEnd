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
    public class UpdateTestInstanceStatusByIdCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public int Status { get; set; }

        public class UpdateTestInstanceStatusByIdCommandHandler : IRequestHandler<UpdateTestInstanceStatusByIdCommand, Response<bool>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public UpdateTestInstanceStatusByIdCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<bool>> Handle(UpdateTestInstanceStatusByIdCommand command, CancellationToken cancellationToken)
            {
                var item = await _testinstanceRepository.GetByIdAsync(command.Id);

                if (item == null)
                {
                    throw new ApiException($"Test Instance Not Found.");
                }
                else
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
                    
                    await _testinstanceRepository.UpdateAsync(item);
                    return new Response<bool>(true);
                }
            }
        }

    }
}
