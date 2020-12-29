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
	public class UpdateChoiceSubmissionCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public Choice Choice { get; set; }
		public int ChoiceId { get; set; }
		public SingleQuestionSubmission SingleQuestionSubmission { get; set; }
		public int SingleQuestionSubmissionId { get; set; }

        public class UpdateChoiceSubmissionCommandHandler : IRequestHandler<UpdateChoiceSubmissionCommand, Response<int>>
        {
            private readonly IChoiceSubmissionRepositoryAsync _choicesubmissionRepository;
            public UpdateChoiceSubmissionCommandHandler(IChoiceSubmissionRepositoryAsync choicesubmissionRepository)
            {
                _choicesubmissionRepository = choicesubmissionRepository;
            }
            public async Task<Response<int>> Handle(UpdateChoiceSubmissionCommand command, CancellationToken cancellationToken)
            {
                var choicesubmission = await _choicesubmissionRepository.GetByIdAsync(command.Id);

                if (choicesubmission == null)
                {
                    throw new ApiException($"ChoiceSubmission Not Found.");
                }
                else
                {
				choicesubmission.Choice = command.Choice;
				choicesubmission.ChoiceId = command.ChoiceId;
				choicesubmission.SingleQuestionSubmission = command.SingleQuestionSubmission;
				choicesubmission.SingleQuestionSubmissionId = command.SingleQuestionSubmissionId; 

                    await _choicesubmissionRepository.UpdateAsync(choicesubmission);
                    return new Response<int>(choicesubmission.Id);
                }
            }
        }

    }
}
