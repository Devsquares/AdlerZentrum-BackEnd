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
    public class GetAdlerCardSubmissionByIdQuery : IRequest<Response<Domain.Entities.AdlerCardSubmission>>
    {
        public int Id { get; set; }
        public class GetAdlerCardSubmissionByIdQueryHandler : IRequestHandler<GetAdlerCardSubmissionByIdQuery, Response<Domain.Entities.AdlerCardSubmission>>
        {
            private readonly IAdlerCardSubmissionRepositoryAsync _adlercardsubmissionRepository;
            public GetAdlerCardSubmissionByIdQueryHandler(IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository)
            {
                _adlercardsubmissionRepository = adlercardsubmissionRepository;
            }
            public async Task<Response<Domain.Entities.AdlerCardSubmission>> Handle(GetAdlerCardSubmissionByIdQuery query, CancellationToken cancellationToken)
            {
                var adlercardsubmission = await _adlercardsubmissionRepository.GetByIdAsync(query.Id);
                if (adlercardsubmission == null) throw new ApiException($"AdlerCardSubmission Not Found.");
                return new Response<Domain.Entities.AdlerCardSubmission>(adlercardsubmission);
            }
        }
    }
}
