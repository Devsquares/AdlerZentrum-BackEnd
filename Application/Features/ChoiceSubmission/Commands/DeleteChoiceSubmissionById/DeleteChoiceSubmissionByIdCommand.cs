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
    public class DeleteChoiceSubmissionByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteChoiceSubmissionByIdCommandHandler : IRequestHandler<DeleteChoiceSubmissionByIdCommand, Response<int>>
        {
            private readonly IChoiceSubmissionRepositoryAsync _choicesubmissionRepository;
            public DeleteChoiceSubmissionByIdCommandHandler(IChoiceSubmissionRepositoryAsync choicesubmissionRepository)
            {
                _choicesubmissionRepository = choicesubmissionRepository;
            }
            public async Task<Response<int>> Handle(DeleteChoiceSubmissionByIdCommand command, CancellationToken cancellationToken)
            {
                var choicesubmission = await _choicesubmissionRepository.GetByIdAsync(command.Id);
                if (choicesubmission == null) throw new ApiException($"ChoiceSubmission Not Found.");
                await _choicesubmissionRepository.DeleteAsync(choicesubmission);
                return new Response<int>(choicesubmission.Id);
            }
        }
    }
}
