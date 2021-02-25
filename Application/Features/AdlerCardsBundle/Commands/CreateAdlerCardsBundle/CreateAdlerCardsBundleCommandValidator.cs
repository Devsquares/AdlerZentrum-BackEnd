using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateAdlerCardsBundleCommandValidator : AbstractValidator<CreateAdlerCardsBundleCommand>
    {
        private readonly IAdlerCardsBundleRepositoryAsync adlercardsbundleRepository;

        public CreateAdlerCardsBundleCommandValidator(IAdlerCardsBundleRepositoryAsync adlercardsbundleRepository)
        {
            this.adlercardsbundleRepository = adlercardsbundleRepository;
        }
    }
}
