using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateAdlerCardSubmissionCommandValidator : AbstractValidator<UpdateAdlerCardSubmissionCommand>
    {
        private readonly IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository;

        public UpdateAdlerCardSubmissionCommandValidator(IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository)
        {
            this.adlercardsubmissionRepository = adlercardsubmissionRepository;

        }
    }
}
