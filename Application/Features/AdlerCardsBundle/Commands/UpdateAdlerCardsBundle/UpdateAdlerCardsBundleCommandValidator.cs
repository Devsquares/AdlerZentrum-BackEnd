using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateAdlerCardsBundleCommandValidator : AbstractValidator<UpdateAdlerCardsBundleCommand>
    {
        private readonly IAdlerCardsBundleRepositoryAsync adlercardsbundleRepository;

        public UpdateAdlerCardsBundleCommandValidator(IAdlerCardsBundleRepositoryAsync adlercardsbundleRepository)
        {
            this.adlercardsbundleRepository = adlercardsbundleRepository;

        }
    }
}
