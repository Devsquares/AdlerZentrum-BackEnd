using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateChoiceSubmissionCommandValidator : AbstractValidator<UpdateChoiceSubmissionCommand>
    {
        private readonly IChoiceSubmissionRepositoryAsync choicesubmissionRepository;

        public UpdateChoiceSubmissionCommandValidator(IChoiceSubmissionRepositoryAsync choicesubmissionRepository)
        {
            this.choicesubmissionRepository = choicesubmissionRepository;

        }
    }
}
