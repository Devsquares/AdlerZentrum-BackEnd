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
    public class GetSingleQuestionSubmissionByIdQuery : IRequest<Response<Domain.Entities.SingleQuestionSubmission>>
    {
        public int Id { get; set; }
        public class GetSingleQuestionSubmissionByIdQueryHandler : IRequestHandler<GetSingleQuestionSubmissionByIdQuery, Response<Domain.Entities.SingleQuestionSubmission>>
        {
            private readonly ISingleQuestionSubmissionRepositoryAsync _singlequestionsubmissionRepository;
            public GetSingleQuestionSubmissionByIdQueryHandler(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository)
            {
                _singlequestionsubmissionRepository = singlequestionsubmissionRepository;
            }
            public async Task<Response<Domain.Entities.SingleQuestionSubmission>> Handle(GetSingleQuestionSubmissionByIdQuery query, CancellationToken cancellationToken)
            {
                var singlequestionsubmission = await _singlequestionsubmissionRepository.GetByIdAsync(query.Id);
                if (singlequestionsubmission == null) throw new ApiException($"SingleQuestionSubmission Not Found.");
                return new Response<Domain.Entities.SingleQuestionSubmission>(singlequestionsubmission);
            }
        }
    }
}
