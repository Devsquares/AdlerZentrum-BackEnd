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
    public class DeleteDisqualificationRequestByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteDisqualificationRequestByIdCommandHandler : IRequestHandler<DeleteDisqualificationRequestByIdCommand, Response<int>>
        {
            private readonly IDisqualificationRequestRepositoryAsync _disqualificationrequestRepository;
            public DeleteDisqualificationRequestByIdCommandHandler(IDisqualificationRequestRepositoryAsync disqualificationrequestRepository)
            {
                _disqualificationrequestRepository = disqualificationrequestRepository;
            }
            public async Task<Response<int>> Handle(DeleteDisqualificationRequestByIdCommand command, CancellationToken cancellationToken)
            {
                var disqualificationrequest = await _disqualificationrequestRepository.GetByIdAsync(command.Id);
                if (disqualificationrequest == null) throw new ApiException($"DisqualificationRequest Not Found.");
                await _disqualificationrequestRepository.DeleteAsync(disqualificationrequest);
                return new Response<int>(disqualificationrequest.Id);
            }
        }
    }
}
