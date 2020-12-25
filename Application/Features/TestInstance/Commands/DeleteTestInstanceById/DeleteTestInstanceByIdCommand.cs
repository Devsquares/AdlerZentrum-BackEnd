using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TestInstance.Commands.DeleteTestInstanceById
{
    public class DeleteTestInstanceByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteTestInstanceByIdCommandHandler : IRequestHandler<DeleteTestInstanceByIdCommand, Response<int>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public DeleteTestInstanceByIdCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<int>> Handle(DeleteTestInstanceByIdCommand command, CancellationToken cancellationToken)
            {
                var testinstance = await _testinstanceRepository.GetByIdAsync(command.Id);
                if (testinstance == null) throw new ApiException($"TestInstance Not Found.");
                await _testinstanceRepository.DeleteAsync(testinstance);
                return new Response<int>(testinstance.Id);
            }
        }
    }
}
