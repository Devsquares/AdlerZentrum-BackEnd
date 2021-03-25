using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateEmailTypeCommandValidator : AbstractValidator<CreateEmailTypeCommand>
    {
        private readonly IEmailTypeRepositoryAsync emailtypeRepository;

        public CreateEmailTypeCommandValidator(IEmailTypeRepositoryAsync emailtypeRepository)
        {
            this.emailtypeRepository = emailtypeRepository;
        }
    }
}
