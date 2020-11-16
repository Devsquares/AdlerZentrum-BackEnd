using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Address.Commands.UpdateAddress
{
    public class UpdateAddressCommandValidator :  AbstractValidator<UpdateAddressCommand>
    {
        private readonly IAddressRepositoryAsync addressRepository;

        public UpdateAddressCommandValidator(IAddressRepositoryAsync addressRepository)
        {
        this.addressRepository = addressRepository;

        RuleFor(p => p.Address1)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();

        RuleFor(p => p.PostCode)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();

        }
    }
}
