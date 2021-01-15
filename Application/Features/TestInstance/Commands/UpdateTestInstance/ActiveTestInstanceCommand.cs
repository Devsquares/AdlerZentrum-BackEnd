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
    public class ActiveTestInstanceCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        public class AssginTeacherTestInstanceCommandHandler : IRequestHandler<AssginTeacherTestInstanceCommand, Response<int>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public AssginTeacherTestInstanceCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<int>> Handle(AssginTeacherTestInstanceCommand command, CancellationToken cancellationToken)
            {
                var testinstance = await _testinstanceRepository.GetByIdAsync(command.Id);

                if (testinstance == null)
                {
                    throw new ApiException($"Test Instance Not Found.");
                }
                else
                {
                    testinstance.Status = (int)TestInstanceEnum.Pending;
                    testinstance.StartDate = DateTime.Now;

                    await _testinstanceRepository.UpdateAsync(testinstance);
                    return new Response<int>(testinstance.Id);
                }
            }
        }

    }
}
