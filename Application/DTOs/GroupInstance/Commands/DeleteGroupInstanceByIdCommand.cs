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

namespace Application.DTOs.GroupInstance.Commands
{
    public class DeleteGroupInstanceByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteGroupInstanceByIdCommandHandler : IRequestHandler<DeleteGroupInstanceByIdCommand, Response<int>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            public DeleteGroupInstanceByIdCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
            }
            public async Task<Response<int>> Handle(DeleteGroupInstanceByIdCommand command, CancellationToken cancellationToken)
            {
                var groupInstance = await _groupInstanceRepositoryAsync.GetByIdAsync(command.Id);
                if (groupInstance == null) throw new ApiException($"Group Not Found.");
                await _groupInstanceRepositoryAsync.DeleteAsync(groupInstance);
                return new Response<int>(groupInstance.Id);
            }
        }
    }
}
