using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class CreateGroupConditionPromoCodeCommandValidator : AbstractValidator<CreateGroupConditionPromoCodeCommand>
    {
        private readonly IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository;

        public CreateGroupConditionPromoCodeCommandValidator(IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository)
        {
            this.groupconditionpromocodeRepository = groupconditionpromocodeRepository;
        }
    }
}
