using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class DeleteSublevelByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteSublevelByIdCommandHandler : IRequestHandler<DeleteSublevelByIdCommand, Response<int>>
        {
            private readonly ISublevelRepositoryAsync _sublevelRepository;
            public DeleteSublevelByIdCommandHandler(ISublevelRepositoryAsync sublevelRepository)
            {
                _sublevelRepository = sublevelRepository;
            }
            public async Task<Response<int>> Handle(DeleteSublevelByIdCommand command, CancellationToken cancellationToken)
            {
                var sublevel = await _sublevelRepository.GetByIdAsync(command.Id);
                if (sublevel == null) throw new ApiException($"Sublevel Not Found.");
                await _sublevelRepository.DeleteAsync(sublevel);
                return new Response<int>(sublevel.Id);
            }
        }
    }
}
