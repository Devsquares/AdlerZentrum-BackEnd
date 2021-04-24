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
    public class GetAllAdlerCardsForStudent : IRequest<PagedResponse<IEnumerable<AdlerCardModel>>>
    {
        public int AdlerCardUnitId { get; set; }
        public string StudentId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public class GetAllAdlerCardsForStudentHandler : IRequestHandler<GetAllAdlerCardsForStudent, PagedResponse<IEnumerable<AdlerCardModel>>>
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

            public async Task<PagedResponse<IEnumerable<AdlerCardModel>>> Handle(GetAllAdlerCardsForStudent request, CancellationToken cancellationToken)
            {
                var adlercardunit = _adlerCardsUnitRepositoryAsync.GetByIdAsync(request.AdlerCardUnitId).Result;
                if(adlercardunit == null)
                {
                    throw new ApiException("No AdlerCard Unit Found");
                }
                if (request.pageNumber == 0) request.pageNumber = 1;
                if (request.pageSize == 0) request.pageSize = 10;
                int count = 0;
                var adlerCards = _adlercardRepository.GetAdlerCardsForStudent(request.pageNumber, request.pageSize,request.StudentId, request.AdlerCardUnitId,out count);
                return new PagedResponse<IEnumerable<AdlerCardModel>>(adlerCards, request.pageNumber, request.pageSize,count);
            }
        }
    }
}
