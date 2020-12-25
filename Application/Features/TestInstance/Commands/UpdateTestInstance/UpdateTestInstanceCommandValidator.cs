using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.TestInstance.Commands.UpdateTestInstance
{
    public class UpdateTestInstanceCommandValidator : AbstractValidator<UpdateTestInstanceCommand>
    {
        private readonly ITestInstanceRepositoryAsync testinstanceRepository;

        public UpdateTestInstanceCommandValidator(ITestInstanceRepositoryAsync testinstanceRepository)
        {
            this.testinstanceRepository = testinstanceRepository;

        }
    }
}
