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
    public class DeleteSingleQuestionSubmissionByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteSingleQuestionSubmissionByIdCommandHandler : IRequestHandler<DeleteSingleQuestionSubmissionByIdCommand, Response<int>>
        {
            private readonly ISingleQuestionSubmissionRepositoryAsync _singlequestionsubmissionRepository;
            public DeleteSingleQuestionSubmissionByIdCommandHandler(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository)
            {
                _singlequestionsubmissionRepository = singlequestionsubmissionRepository;
            }
            public async Task<Response<int>> Handle(DeleteSingleQuestionSubmissionByIdCommand command, CancellationToken cancellationToken)
            {
                var singlequestionsubmission = await _singlequestionsubmissionRepository.GetByIdAsync(command.Id);
                if (singlequestionsubmission == null) throw new ApiException($"SingleQuestionSubmission Not Found.");
                await _singlequestionsubmissionRepository.DeleteAsync(singlequestionsubmission);
                return new Response<int>(singlequestionsubmission.Id);
            }
        }
    }
}
