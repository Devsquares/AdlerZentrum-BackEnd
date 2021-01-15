using Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.GroupConditionDetails.Commands.UpdateGroupConditionDetails
{
    public class UpdateGroupConditionDetailsCommandValidator : AbstractValidator<UpdateGroupConditionDetailsCommand>
    {
        private readonly IGroupConditionDetailsRepositoryAsync groupconditiondetailsRepository;

        public UpdateGroupConditionDetailsCommandValidator(IGroupConditionDetailsRepositoryAsync groupconditiondetailsRepository)
        {
            this.groupconditiondetailsRepository = groupconditiondetailsRepository;

        }
    }
}
