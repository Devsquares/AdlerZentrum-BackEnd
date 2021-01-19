using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.InterestedStudent.Commands.CreateInterestedStudent
{
    public class CreateInterestedStudentCommandValidator : AbstractValidator<CreateInterestedStudentCommand>
    {
        private readonly IInterestedStudentRepositoryAsync interestedstudentRepository;

        public CreateInterestedStudentCommandValidator(IInterestedStudentRepositoryAsync interestedstudentRepository)
        {
            this.interestedstudentRepository = interestedstudentRepository;
        }
    }
}
