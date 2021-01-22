using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Bug.Commands.UpdateBug
{
    public class UpdateBugCommandValidator : AbstractValidator<UpdateBugCommand>
    {
        private readonly IBugRepositoryAsync bugRepository;

        public UpdateBugCommandValidator(IBugRepositoryAsync bugRepository)
        {
            this.bugRepository = bugRepository;

        }
    }
}
