using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.AdlerCardBundleStudent.Commands.CreateAdlerCardBundleStudent
{
    public class CreateAdlerCardBundleStudentCommandValidator : AbstractValidator<CreateAdlerCardBundleStudentCommand>
    {
        private readonly IAdlerCardBundleStudentRepositoryAsync adlercardbundlestudentRepository;

        public CreateAdlerCardBundleStudentCommandValidator(IAdlerCardBundleStudentRepositoryAsync adlercardbundlestudentRepository)
        {
            this.adlercardbundlestudentRepository = adlercardbundlestudentRepository;
        }
    }
}
