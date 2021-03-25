using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateBugCommandValidator : AbstractValidator<CreateBugCommand>
    {
        private readonly IBugRepositoryAsync bugRepository;

        public CreateBugCommandValidator(IBugRepositoryAsync bugRepository)
        {
            this.bugRepository = bugRepository;
        }
    }
}
