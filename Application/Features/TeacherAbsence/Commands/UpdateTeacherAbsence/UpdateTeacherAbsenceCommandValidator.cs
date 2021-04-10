using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.TeacherAbsence.Commands.UpdateTeacherAbsence
{
    public class UpdateTeacherAbsenceCommandValidator : AbstractValidator<UpdateTeacherAbsenceCommand>
    {
        private readonly ITeacherAbsenceRepositoryAsync teacherabsenceRepository;

        public UpdateTeacherAbsenceCommandValidator(ITeacherAbsenceRepositoryAsync teacherabsenceRepository)
        {
            this.teacherabsenceRepository = teacherabsenceRepository;

        }
    }
}
