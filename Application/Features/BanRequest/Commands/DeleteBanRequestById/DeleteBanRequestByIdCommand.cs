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
    public class DeleteBanRequestByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteBanRequestByIdCommandHandler : IRequestHandler<DeleteBanRequestByIdCommand, Response<int>>
        {
            private readonly IBanRequestRepositoryAsync _banrequestRepository;
            public DeleteBanRequestByIdCommandHandler(IBanRequestRepositoryAsync banrequestRepository)
            {
                _banrequestRepository = banrequestRepository;
            }
            public async Task<Response<int>> Handle(DeleteBanRequestByIdCommand command, CancellationToken cancellationToken)
            {
                var banrequest = await _banrequestRepository.GetByIdAsync(command.Id);
                if (banrequest == null) throw new ApiException($"BanRequest Not Found.");
                await _banrequestRepository.DeleteAsync(banrequest);
                return new Response<int>(banrequest.Id);
            }
        }
    }
}
