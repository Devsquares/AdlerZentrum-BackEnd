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

namespace Application.DTOs.Level.Commands
{
    public class DeleteLevelByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteLevelByIdCommandHandler : IRequestHandler<DeleteLevelByIdCommand, Response<int>>
        {
            private readonly ILevelRepositoryAsync _levelRepositoryAsync;
            public DeleteLevelByIdCommandHandler(ILevelRepositoryAsync levelRepositoryAsync)
            {
                _levelRepositoryAsync = levelRepositoryAsync;
            }
            public async Task<Response<int>> Handle(DeleteLevelByIdCommand command, CancellationToken cancellationToken)
            {
                var Level = await _levelRepositoryAsync.GetByIdAsync(command.Id);
                if (Level == null) throw new ApiException($"Level Not Found.");
                await _levelRepositoryAsync.DeleteAsync(Level);
                return new Response<int>(Level.Id);
            }
        }
    }
}
