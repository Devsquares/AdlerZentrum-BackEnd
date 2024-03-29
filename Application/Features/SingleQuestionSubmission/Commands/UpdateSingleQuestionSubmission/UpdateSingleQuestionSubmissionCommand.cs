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
    public class UpdateSingleQuestionSubmissionCommand : IRequest<Response<int?>>
    {
        public int Id { get; set; }
        public bool RightAnswer { get; set; }
        public double Points { get; set; }
        public string CorrectionText { get; set; }

        public class UpdateSingleQuestionSubmissionCommandHandler : IRequestHandler<UpdateSingleQuestionSubmissionCommand, Response<int?>>
        {
            private readonly ISingleQuestionSubmissionRepositoryAsync _singlequestionsubmissionRepository;
            public UpdateSingleQuestionSubmissionCommandHandler(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository)
            {
                _singlequestionsubmissionRepository = singlequestionsubmissionRepository;
            }
            public async Task<Response<int?>> Handle(UpdateSingleQuestionSubmissionCommand command, CancellationToken cancellationToken)
            {
                var singlequestionsubmission = await _singlequestionsubmissionRepository.GetByIdAsync(command.Id);

                if (singlequestionsubmission == null)
                {
                    throw new ApiException($"Single Question Submission Not Found.");
                }
                else
                {
                    singlequestionsubmission.RightAnswer = command.RightAnswer;
                    singlequestionsubmission.Points = command.Points;
                    singlequestionsubmission.CorrectionText = command.CorrectionText;
                    singlequestionsubmission.Corrected = true;

                    await _singlequestionsubmissionRepository.UpdateAsync(singlequestionsubmission);
                    return new Response<int?>(singlequestionsubmission.TestInstanceId);
                }
            }
        }

    }
}
