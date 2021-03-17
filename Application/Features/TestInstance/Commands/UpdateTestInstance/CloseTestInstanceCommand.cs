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
    public class CloseTestInstanceCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        public class CloseTestInstanceCommandHandler : IRequestHandler<CloseTestInstanceCommand, Response<int>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public CloseTestInstanceCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<int>> Handle(CloseTestInstanceCommand command, CancellationToken cancellationToken)
            {
                var testinstance = await _testinstanceRepository.GetByIdAsync(command.Id);

                if (testinstance == null)
                {
                    throw new ApiException($"Test Instance Not Found.");
                }
                else
                {
                    if (testinstance.Status != (int)TestInstanceEnum.Solved)
                    {
                        testinstance.Status = (int)TestInstanceEnum.Missed;
                        testinstance.SubmissionDate = DateTime.Now;
                    }

                    await _testinstanceRepository.UpdateAsync(testinstance);
                    return new Response<int>(testinstance.Id);
                }
            }
        }

    }
}
