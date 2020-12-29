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
    public class GetChoiceSubmissionByIdQuery : IRequest<Response<Domain.Entities.ChoiceSubmission>>
    {
        public int Id { get; set; }
        public class GetChoiceSubmissionByIdQueryHandler : IRequestHandler<GetChoiceSubmissionByIdQuery, Response<Domain.Entities.ChoiceSubmission>>
        {
            private readonly IChoiceSubmissionRepositoryAsync _choicesubmissionRepository;
            public GetChoiceSubmissionByIdQueryHandler(IChoiceSubmissionRepositoryAsync choicesubmissionRepository)
            {
                _choicesubmissionRepository = choicesubmissionRepository;
            }
            public async Task<Response<Domain.Entities.ChoiceSubmission>> Handle(GetChoiceSubmissionByIdQuery query, CancellationToken cancellationToken)
            {
                var choicesubmission = await _choicesubmissionRepository.GetByIdAsync(query.Id);
                if (choicesubmission == null) throw new ApiException($"ChoiceSubmission Not Found.");
                return new Response<Domain.Entities.ChoiceSubmission>(choicesubmission);
            }
        }
    }
}
