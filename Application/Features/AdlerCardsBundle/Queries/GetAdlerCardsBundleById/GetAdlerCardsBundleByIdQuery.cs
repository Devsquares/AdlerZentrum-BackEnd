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
    public class GetAdlerCardsBundleByIdQuery : IRequest<Response<Domain.Entities.AdlerCardsBundle>>
    {
        public int Id { get; set; }
        public class GetAdlerCardsBundleByIdQueryHandler : IRequestHandler<GetAdlerCardsBundleByIdQuery, Response<Domain.Entities.AdlerCardsBundle>>
        {
            private readonly IAdlerCardsBundleRepositoryAsync _adlercardsbundleRepository;
            public GetAdlerCardsBundleByIdQueryHandler(IAdlerCardsBundleRepositoryAsync adlercardsbundleRepository)
            {
                _adlercardsbundleRepository = adlercardsbundleRepository;
            }
            public async Task<Response<Domain.Entities.AdlerCardsBundle>> Handle(GetAdlerCardsBundleByIdQuery query, CancellationToken cancellationToken)
            {
                var adlercardsbundle = await _adlercardsbundleRepository.GetByIdAsync(query.Id);
                if (adlercardsbundle == null) throw new ApiException($"AdlerCardsBundle Not Found.");
                return new Response<Domain.Entities.AdlerCardsBundle>(adlercardsbundle);
            }
        }
    }
}
