using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateSingleQuestionSubmissionCommandValidator : AbstractValidator<CreateSingleQuestionSubmissionCommand>
    {
        private readonly ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository;

        public CreateSingleQuestionSubmissionCommandValidator(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository)
        {
            this.singlequestionsubmissionRepository = singlequestionsubmissionRepository;
        }
    }
}
