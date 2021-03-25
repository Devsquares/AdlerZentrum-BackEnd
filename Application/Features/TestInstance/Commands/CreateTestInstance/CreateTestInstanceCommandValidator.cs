using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateTestInstanceCommandValidator : AbstractValidator<TestInstanceSolutionCommand>
    {
        private readonly ITestInstanceRepositoryAsync testinstanceRepository;

        public CreateTestInstanceCommandValidator(ITestInstanceRepositoryAsync testinstanceRepository)
        {
            this.testinstanceRepository = testinstanceRepository;
        }
    }
}
