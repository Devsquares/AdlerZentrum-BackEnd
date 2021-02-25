using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateAdlerCardsUnitCommandValidator : AbstractValidator<CreateAdlerCardsUnitCommand>
    {
        private readonly IAdlerCardsUnitRepositoryAsync adlercardsunitRepository;

        public CreateAdlerCardsUnitCommandValidator(IAdlerCardsUnitRepositoryAsync adlercardsunitRepository)
        {
            this.adlercardsunitRepository = adlercardsunitRepository;
        }
    }
}
