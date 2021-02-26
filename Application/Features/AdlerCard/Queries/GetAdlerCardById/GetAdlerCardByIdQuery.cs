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
    public class GetAdlerCardByIdQuery : IRequest<Response<Domain.Entities.AdlerCard>>
    {
        public int Id { get; set; }
        public class GetAdlerCardByIdQueryHandler : IRequestHandler<GetAdlerCardByIdQuery, Response<Domain.Entities.AdlerCard>>
        {
            private readonly IAdlerCardRepositoryAsync _adlercardRepository;
            public GetAdlerCardByIdQueryHandler(IAdlerCardRepositoryAsync adlercardRepository)
            {
                _adlercardRepository = adlercardRepository;
            }
            public async Task<Response<Domain.Entities.AdlerCard>> Handle(GetAdlerCardByIdQuery query, CancellationToken cancellationToken)
            {
                var adlercard = await _adlercardRepository.GetByIdAsync(query.Id);
                if (adlercard == null) throw new ApiException($"AdlerCard Not Found.");
                return new Response<Domain.Entities.AdlerCard>(adlercard);
            }
        }
    }
}
