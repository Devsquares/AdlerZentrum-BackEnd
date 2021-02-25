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
    public class GetAdlerCardsUnitByIdQuery : IRequest<Response<Domain.Entities.AdlerCardsUnit>>
    {
        public int Id { get; set; }
        public class GetAdlerCardsUnitByIdQueryHandler : IRequestHandler<GetAdlerCardsUnitByIdQuery, Response<Domain.Entities.AdlerCardsUnit>>
        {
            private readonly IAdlerCardsUnitRepositoryAsync _adlercardsunitRepository;
            public GetAdlerCardsUnitByIdQueryHandler(IAdlerCardsUnitRepositoryAsync adlercardsunitRepository)
            {
                _adlercardsunitRepository = adlercardsunitRepository;
            }
            public async Task<Response<Domain.Entities.AdlerCardsUnit>> Handle(GetAdlerCardsUnitByIdQuery query, CancellationToken cancellationToken)
            {
                var adlercardsunit = await _adlercardsunitRepository.GetByIdAsync(query.Id);
                if (adlercardsunit == null) throw new ApiException($"AdlerCardsUnit Not Found.");
                return new Response<Domain.Entities.AdlerCardsUnit>(adlercardsunit);
            }
        }
    }
}
