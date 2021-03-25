using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdatePromoCodeInstanceCommandValidator : AbstractValidator<UpdatePromoCodeInstanceCommand>
    {
        private readonly IPromoCodeInstanceRepositoryAsync promocodeinstanceRepository;

        public UpdatePromoCodeInstanceCommandValidator(IPromoCodeInstanceRepositoryAsync promocodeinstanceRepository)
        {
            this.promocodeinstanceRepository = promocodeinstanceRepository;

        }
    }
}
