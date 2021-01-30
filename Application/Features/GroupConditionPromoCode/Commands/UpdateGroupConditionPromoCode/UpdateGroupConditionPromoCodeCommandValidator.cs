using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class UpdateGroupConditionPromoCodeCommandValidator : AbstractValidator<UpdateGroupConditionPromoCodeCommand>
    {
        private readonly IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository;

        public UpdateGroupConditionPromoCodeCommandValidator(IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository)
        {
            this.groupconditionpromocodeRepository = groupconditionpromocodeRepository;

        }
    }
}
