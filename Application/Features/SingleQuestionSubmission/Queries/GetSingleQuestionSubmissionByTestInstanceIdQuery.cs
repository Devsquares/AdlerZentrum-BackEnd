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
    public class GetSingleQuestionSubmissionByTestInstanceIdQuery : IRequest<Response<IReadOnlyList<Domain.Entities.SingleQuestionSubmission>>>
    {
        public int Id { get; set; }
        public class GetSingleQuestionSubmissionByTestInstanceIdQueryHandler : IRequestHandler<GetSingleQuestionSubmissionByTestInstanceIdQuery, Response<IReadOnlyList<Domain.Entities.SingleQuestionSubmission>>>
        {
            private readonly ISingleQuestionSubmissionRepositoryAsync _singlequestionsubmissionRepository;
            public GetSingleQuestionSubmissionByTestInstanceIdQueryHandler(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository)
            {
                _singlequestionsubmissionRepository = singlequestionsubmissionRepository;
            }
            public async Task<Response<IReadOnlyList<Domain.Entities.SingleQuestionSubmission>>> Handle(GetSingleQuestionSubmissionByTestInstanceIdQuery query, CancellationToken cancellationToken)
            {
                var singlequestionsubmission = await _singlequestionsubmissionRepository.GetByTestInstanceIdAsync(query.Id);
                if (singlequestionsubmission == null) throw new ApiException($"SingleQuestionSubmission Not Found.");
                return new Response<IReadOnlyList<Domain.Entities.SingleQuestionSubmission>>(singlequestionsubmission);
            }
        }
    }
}
