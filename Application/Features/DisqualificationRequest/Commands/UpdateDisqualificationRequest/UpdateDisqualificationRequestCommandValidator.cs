using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features 
{
    public class UpdateDisqualificationRequestCommandValidator : AbstractValidator<UpdateDisqualificationRequestCommand>
    {
        private readonly IDisqualificationRequestRepositoryAsync disqualificationrequestRepository;

        public UpdateDisqualificationRequestCommandValidator(IDisqualificationRequestRepositoryAsync disqualificationrequestRepository)
        {
            this.disqualificationrequestRepository = disqualificationrequestRepository;

        }
    }
}
