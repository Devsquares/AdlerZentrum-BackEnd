using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.GroupConditionDetails.Commands.CreateGroupConditionDetails
{
    public class CreateGroupConditionDetailsCommandValidator : AbstractValidator<CreateGroupConditionDetailsCommand>
    {
        private readonly IGroupConditionDetailsRepositoryAsync groupconditiondetailsRepository;

        public CreateGroupConditionDetailsCommandValidator(IGroupConditionDetailsRepositoryAsync groupconditiondetailsRepository)
        {
            this.groupconditiondetailsRepository = groupconditiondetailsRepository;
        }
    }
}
