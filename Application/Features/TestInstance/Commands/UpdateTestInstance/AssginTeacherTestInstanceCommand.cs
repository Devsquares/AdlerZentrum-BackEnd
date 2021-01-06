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
    public class AssginTeacherTestInstanceCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string CorrectionTeacherId { get; set; }

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
                    throw new ApiException($"TestInstance Not Found.");
                }
                else
                {
                    testinstance.CorrectionTeacherId = command.CorrectionTeacherId;

                    await _testinstanceRepository.UpdateAsync(testinstance);
                    return new Response<int>(testinstance.Id);
                }
            }
        }

    }
}
