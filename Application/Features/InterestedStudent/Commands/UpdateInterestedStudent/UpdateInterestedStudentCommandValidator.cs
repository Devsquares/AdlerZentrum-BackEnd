using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateInterestedStudentCommandValidator : AbstractValidator<UpdateInterestedStudentCommand>
    {
        private readonly IInterestedStudentRepositoryAsync interestedstudentRepository;

        public UpdateInterestedStudentCommandValidator(IInterestedStudentRepositoryAsync interestedstudentRepository)
        {
            this.interestedstudentRepository = interestedstudentRepository;

        }
    }
}
