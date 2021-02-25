using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateAdlerCardCommandValidator : AbstractValidator<CreateAdlerCardCommand>
    {
        private readonly IAdlerCardRepositoryAsync adlercardRepository;

        public CreateAdlerCardCommandValidator(IAdlerCardRepositoryAsync adlercardRepository)
        {
            this.adlercardRepository = adlercardRepository;
        }
    }
}
