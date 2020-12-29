using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateSingleQuestionSubmissionCommandValidator : AbstractValidator<UpdateSingleQuestionSubmissionCommand>
    {
        private readonly ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository;

        public UpdateSingleQuestionSubmissionCommandValidator(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository)
        {
            this.singlequestionsubmissionRepository = singlequestionsubmissionRepository;

        }
    }
}
