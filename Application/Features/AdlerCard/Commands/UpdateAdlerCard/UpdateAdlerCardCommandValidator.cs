using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateAdlerCardCommandValidator : AbstractValidator<UpdateAdlerCardCommand>
    {
        private readonly IAdlerCardRepositoryAsync adlercardRepository;

        public UpdateAdlerCardCommandValidator(IAdlerCardRepositoryAsync adlercardRepository)
        {
            this.adlercardRepository = adlercardRepository;

        }
    }
}
