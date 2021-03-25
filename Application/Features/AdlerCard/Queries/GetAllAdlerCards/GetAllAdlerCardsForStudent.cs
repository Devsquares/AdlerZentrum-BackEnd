using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetAllAdlerCardsForStudent : IRequest<Response<IEnumerable<AdlerCardModel>>>
    {
        public int AdlerCardUnitId { get; set; }
        public string StudentId { get; set; }
        public class GetAllAdlerCardsForStudentHandler : IRequestHandler<GetAllAdlerCardsForStudent, Response<IEnumerable<AdlerCardModel>>>
        {
            private readonly IAdlerCardRepositoryAsync _adlercardRepository;
            private readonly IAdlerCardSubmissionRepositoryAsync _adlerCardSubmissionRepositoryAsync;
            private readonly IAdlerCardsUnitRepositoryAsync _adlerCardsUnitRepositoryAsync;
            public GetAllAdlerCardsForStudentHandler(IAdlerCardRepositoryAsync adlercardRepository, IAdlerCardSubmissionRepositoryAsync adlerCardSubmissionRepositoryAsync,
                IAdlerCardsUnitRepositoryAsync adlerCardsUnitRepositoryAsync)
            {
                _adlercardRepository = adlercardRepository;
                _adlerCardSubmissionRepositoryAsync = adlerCardSubmissionRepositoryAsync;
                _adlerCardsUnitRepositoryAsync = adlerCardsUnitRepositoryAsync;
            }

            public async Task<Response<IEnumerable<AdlerCardModel>>> Handle(GetAllAdlerCardsForStudent request, CancellationToken cancellationToken)
            {
                var adlercardunit = _adlerCardsUnitRepositoryAsync.GetByIdAsync(request.AdlerCardUnitId).Result;
                if(adlercardunit == null)
                {
                    throw new ApiException("No AdlerCard Unit Found");
                }
                var adlerCards = _adlercardRepository.GetAdlerCardsForStudent(request.StudentId, request.AdlerCardUnitId);
                return new Response<IEnumerable<AdlerCardModel>>(adlerCards);
            }
        }
    }
}
