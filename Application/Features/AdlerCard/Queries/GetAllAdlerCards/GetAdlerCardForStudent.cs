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

namespace Application.Features.AdlerCard.Queries.GetAllAdlerCards
{
    public class GetAdlerCardForStudent : IRequest<Response<AdlerCardSubmission>>
    {
        public int AdlerCardId { get; set; }
        public string StudentId { get; set; }
        public class GetAdlerCardForStudentHandler : IRequestHandler<GetAdlerCardForStudent, Response<AdlerCardSubmission>>
        {
            private readonly IAdlerCardRepositoryAsync _adlercardRepository;
            private readonly IAdlerCardSubmissionRepositoryAsync _adlerCardSubmissionRepositoryAsync;
            public GetAdlerCardForStudentHandler(IAdlerCardRepositoryAsync adlercardRepository, IAdlerCardSubmissionRepositoryAsync adlerCardSubmissionRepositoryAsync)
            {
                _adlercardRepository = adlercardRepository;
                _adlerCardSubmissionRepositoryAsync = adlerCardSubmissionRepositoryAsync;
            }

            public async Task<Response<AdlerCardSubmission>> Handle(GetAdlerCardForStudent request, CancellationToken cancellationToken)
            {
                var adlercard = _adlercardRepository.GetByIdAsync(request.AdlerCardId).Result;
                if(adlercard == null)
                {
                    throw new ApiException("No AdlerCard Found");
                }
                var adlerCardSub = _adlerCardSubmissionRepositoryAsync.GetAdlerCardForStudent(request.StudentId, request.AdlerCardId);
                return new Response<AdlerCardSubmission>(adlerCardSub);
            }
        }
    }
}
