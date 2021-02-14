using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.PromoCodeInstance.Commands.CreatePromoCodeInstance
{
    public class CreatePromoCodeInstanceCommandValidator : AbstractValidator<CreatePromoCodeInstanceCommand>
    {
        private readonly IPromoCodeInstanceRepositoryAsync promocodeinstanceRepository;

        public CreatePromoCodeInstanceCommandValidator(IPromoCodeInstanceRepositoryAsync promocodeinstanceRepository)
        {
            this.promocodeinstanceRepository = promocodeinstanceRepository;
        }
    }
}
