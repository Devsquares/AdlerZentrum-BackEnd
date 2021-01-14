using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.EmailType.Commands.UpdateEmailType
{
    public class UpdateEmailTypeCommandValidator : AbstractValidator<UpdateEmailTypeCommand>
    {
        private readonly IEmailTypeRepositoryAsync emailtypeRepository;

        public UpdateEmailTypeCommandValidator(IEmailTypeRepositoryAsync emailtypeRepository)
        {
            this.emailtypeRepository = emailtypeRepository;

        }
    }
}
