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
    public class DeleteSublevelByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteSublevelByIdCommandHandler : IRequestHandler<DeleteSublevelByIdCommand, Response<int>>
        {
            private readonly ISublevelRepositoryAsync _SublevelRepositoryAsync;
            public DeleteSublevelByIdCommandHandler(ISublevelRepositoryAsync SublevelRepositoryAsync)
            {
                _SublevelRepositoryAsync = SublevelRepositoryAsync;
            }
            public async Task<Response<int>> Handle(DeleteSublevelByIdCommand command, CancellationToken cancellationToken)
            {
                var Sublevel = await _SublevelRepositoryAsync.GetByIdAsync(command.Id);
                if (Sublevel == null) throw new ApiException($"Sublevel Not Found.");
                await _SublevelRepositoryAsync.DeleteAsync(Sublevel);
                return new Response<int>(Sublevel.Id);
            }
        }
    }
}
