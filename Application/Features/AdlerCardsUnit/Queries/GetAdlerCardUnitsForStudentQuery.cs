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
    public class GetAdlerCardUnitsForStudentQuery : IRequest<Response<GetAdlerCardUnitsForStudentViewModel>>
    {
        public string StudentId { get; set; }
        public int LevelId { get; set; }
        public int AdlerCardTypeId { get; set; }
        public class GetAdlerCardUnitsForStudentQueryHandler : IRequestHandler<GetAdlerCardUnitsForStudentQuery, Response<GetAdlerCardUnitsForStudentViewModel>>
        {
            private readonly IAdlerCardsUnitRepositoryAsync _adlercardsunitRepository;
            public GetAdlerCardUnitsForStudentQueryHandler(IAdlerCardsUnitRepositoryAsync adlercardsunitRepository)
            {
                _adlercardsunitRepository = adlercardsunitRepository;
            }
            public async Task<Response<GetAdlerCardUnitsForStudentViewModel>> Handle(GetAdlerCardUnitsForStudentQuery query, CancellationToken cancellationToken)
            {
                var adlercardsunit = _adlercardsunitRepository.GetAdlerCardUnitsForStudent(query.StudentId, query.LevelId, query.AdlerCardTypeId);
                if (adlercardsunit == null) throw new ApiException($"AdlerCardsUnit Not Found.");
                // return new Response<GetAdlerCardUnitsForStudentViewModel>(adlercardsunit);
                return null;
            }
        }
    }
}
