using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.TeacherAbsence.Commands.CreateTeacherAbsence
{
    public class CreateTeacherAbsenceCommandValidator : AbstractValidator<CreateTeacherAbsenceCommand>
    {
        private readonly ITeacherAbsenceRepositoryAsync teacherabsenceRepository;

        public CreateTeacherAbsenceCommandValidator(ITeacherAbsenceRepositoryAsync teacherabsenceRepository)
        {
            this.teacherabsenceRepository = teacherabsenceRepository;
        }
    }
}
