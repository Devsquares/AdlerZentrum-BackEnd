using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features 
{
    public class GetAdlerCardUnitsByLevelAndTypeQuery : IRequest<Response<IEnumerable<Domain.Entities.AdlerCardsUnit>>>
    {
        public int LevelId { get; set; }
        public int AdlerCardTypeId { get; set; }
        public class GetAdlerCardUnitsByLevelAndTypeQueryHandler : IRequestHandler<GetAdlerCardUnitsByLevelAndTypeQuery, Response<IEnumerable<Domain.Entities.AdlerCardsUnit>>>
        {
            private readonly IAdlerCardsUnitRepositoryAsync _adlercardsunitRepository;
            public GetAdlerCardUnitsByLevelAndTypeQueryHandler(IAdlerCardsUnitRepositoryAsync adlercardsunitRepository)
            {
                _adlercardsunitRepository = adlercardsunitRepository;
            }
            public async Task<Response<IEnumerable<Domain.Entities.AdlerCardsUnit>>> Handle(GetAdlerCardUnitsByLevelAndTypeQuery query, CancellationToken cancellationToken)
            {
                var adlercardsunit = _adlercardsunitRepository.GetAdlerCardUnitsByLevelAndType( query.LevelId, query.AdlerCardTypeId);
                return new Response<IEnumerable<Domain.Entities.AdlerCardsUnit>>(adlercardsunit);
            }
        }
    }
}
