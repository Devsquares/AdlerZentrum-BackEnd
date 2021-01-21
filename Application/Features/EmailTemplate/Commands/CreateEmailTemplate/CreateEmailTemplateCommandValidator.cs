using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.EmailTemplate.Commands.CreateEmailTemplate
{
    public class CreateEmailTemplateCommandValidator : AbstractValidator<CreateEmailTemplateCommand>
    {
        private readonly IEmailTemplateRepositoryAsync emailtemplateRepository;

        public CreateEmailTemplateCommandValidator(IEmailTemplateRepositoryAsync emailtemplateRepository)
        {
            this.emailtemplateRepository = emailtemplateRepository;
        }
    }
}
