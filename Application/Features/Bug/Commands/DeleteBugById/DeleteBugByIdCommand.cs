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
    public class DeleteBugByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteBugByIdCommandHandler : IRequestHandler<DeleteBugByIdCommand, Response<int>>
        {
            private readonly IBugRepositoryAsync _bugRepository;
            public DeleteBugByIdCommandHandler(IBugRepositoryAsync bugRepository)
            {
                _bugRepository = bugRepository;
            }
            public async Task<Response<int>> Handle(DeleteBugByIdCommand command, CancellationToken cancellationToken)
            {
                var bug = await _bugRepository.GetByIdAsync(command.Id);
                if (bug == null) throw new ApiException($"Bug Not Found.");
                bug.Status = "Archived";
                await _bugRepository.UpdateAsync(bug);
                return new Response<int>(bug.Id);
            }
        }
    }
}
