using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateAdlerCardSubmissionCommandValidator : AbstractValidator<CreateAdlerCardSubmissionCommand>
    {
        private readonly IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository;

        public CreateAdlerCardSubmissionCommandValidator(IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository)
        {
            this.adlercardsubmissionRepository = adlercardsubmissionRepository;
        }
    }
}
