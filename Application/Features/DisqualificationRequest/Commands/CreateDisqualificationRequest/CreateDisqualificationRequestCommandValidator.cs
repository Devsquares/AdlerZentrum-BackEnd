using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateDisqualificationRequestCommandValidator : AbstractValidator<CreateDisqualificationRequestCommand>
    {
        private readonly IDisqualificationRequestRepositoryAsync disqualificationrequestRepository;

        public CreateDisqualificationRequestCommandValidator(IDisqualificationRequestRepositoryAsync disqualificationrequestRepository)
        {
            this.disqualificationrequestRepository = disqualificationrequestRepository;
        }
    }
}
