using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features 
{
    public class UpdateEmailTemplateCommandValidator : AbstractValidator<UpdateEmailTemplateCommand>
    {
        private readonly IEmailTemplateRepositoryAsync emailtemplateRepository;

        public UpdateEmailTemplateCommandValidator(IEmailTemplateRepositoryAsync emailtemplateRepository)
        {
            this.emailtemplateRepository = emailtemplateRepository;

        }
    }
}
