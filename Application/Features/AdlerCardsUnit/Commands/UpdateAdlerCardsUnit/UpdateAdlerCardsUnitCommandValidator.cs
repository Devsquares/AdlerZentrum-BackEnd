using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateAdlerCardsUnitCommandValidator : AbstractValidator<UpdateAdlerCardsUnitCommand>
    {
        private readonly IAdlerCardsUnitRepositoryAsync adlercardsunitRepository;

        public UpdateAdlerCardsUnitCommandValidator(IAdlerCardsUnitRepositoryAsync adlercardsunitRepository)
        {
            this.adlercardsunitRepository = adlercardsunitRepository;

        }
    }
}
