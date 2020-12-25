using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.TestInstance.Commands.CreateTestInstance
{
    public class CreateTestInstanceCommandValidator : AbstractValidator<CreateTestInstanceCommand>
    {
        private readonly ITestInstanceRepositoryAsync testinstanceRepository;

        public CreateTestInstanceCommandValidator(ITestInstanceRepositoryAsync testinstanceRepository)
        {
            this.testinstanceRepository = testinstanceRepository;
        }
    }
}
