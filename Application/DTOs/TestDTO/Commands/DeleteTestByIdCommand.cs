using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DeleteTestByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteTestByIdCommandHandler : IRequestHandler<DeleteTestByIdCommand, Response<int>>
        {
            private readonly ITestRepositoryAsync _TestRepositoryAsync;
            private readonly IMediator _mediator;
            public DeleteTestByIdCommandHandler(ITestRepositoryAsync TestRepositoryAsync, IMediator mediator)
            {
                _TestRepositoryAsync = TestRepositoryAsync;
                _mediator = mediator;
            }
            public async Task<Response<int>> Handle(DeleteTestByIdCommand command, CancellationToken cancellationToken)
            {
                var Test = await _TestRepositoryAsync.GetByIdAsync(command.Id);
                if (Test == null) throw new ApiException($"Test Not Found.");
                if (Test.Status == (int)TestStatusEnum.Final)
                {
                    throw new ApiException($"Test Used cann't be deleted.");
                }
                else
                {
                    await _TestRepositoryAsync.DeleteAsync(Test);
                }
                return new Response<int>(Test.Id);
            }
        }
    }
}
