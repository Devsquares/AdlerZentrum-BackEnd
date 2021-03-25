using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateOverPaymentStudentCommandValidator : AbstractValidator<UpdateOverPaymentStudentCommand>
    {
        private readonly IOverPaymentStudentRepositoryAsync overpaymentstudentRepository;

        public UpdateOverPaymentStudentCommandValidator(IOverPaymentStudentRepositoryAsync overpaymentstudentRepository)
        {
            this.overpaymentstudentRepository = overpaymentstudentRepository;

        }
    }
}
