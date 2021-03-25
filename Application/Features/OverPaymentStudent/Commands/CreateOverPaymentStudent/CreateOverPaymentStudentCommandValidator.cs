using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateOverPaymentStudentCommandValidator : AbstractValidator<CreateOverPaymentStudentCommand>
    {
        private readonly IOverPaymentStudentRepositoryAsync overpaymentstudentRepository;

        public CreateOverPaymentStudentCommandValidator(IOverPaymentStudentRepositoryAsync overpaymentstudentRepository)
        {
            this.overpaymentstudentRepository = overpaymentstudentRepository;
        }
    }
}
