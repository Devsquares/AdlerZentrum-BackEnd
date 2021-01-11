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
    public class GetBanRequestByIdQuery : IRequest<Response<Domain.Entities.BanRequest>>
    {
        public int Id { get; set; }
        public class GetBanRequestByIdQueryHandler : IRequestHandler<GetBanRequestByIdQuery, Response<Domain.Entities.BanRequest>>
        {
            private readonly IBanRequestRepositoryAsync _banrequestRepository;
            public GetBanRequestByIdQueryHandler(IBanRequestRepositoryAsync banrequestRepository)
            {
                _banrequestRepository = banrequestRepository;
            }
            public async Task<Response<Domain.Entities.BanRequest>> Handle(GetBanRequestByIdQuery query, CancellationToken cancellationToken)
            {
                var banrequest = await _banrequestRepository.GetByIdAsync(query.Id);
                if (banrequest == null) throw new ApiException($"BanRequest Not Found.");
                return new Response<Domain.Entities.BanRequest>(banrequest);
            }
        }
    }
}
