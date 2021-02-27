using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.AdlerCardBundleStudent.Commands.UpdateAdlerCardBundleStudent
{
    public class UpdateAdlerCardBundleStudentCommandValidator : AbstractValidator<UpdateAdlerCardBundleStudentCommand>
    {
        private readonly IAdlerCardBundleStudentRepositoryAsync adlercardbundlestudentRepository;

        public UpdateAdlerCardBundleStudentCommandValidator(IAdlerCardBundleStudentRepositoryAsync adlercardbundlestudentRepository)
        {
            this.adlercardbundlestudentRepository = adlercardbundlestudentRepository;

        }
    }
}
