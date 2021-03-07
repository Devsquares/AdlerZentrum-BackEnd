using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateGroupDefinitionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int SubLevelId { get; set; }
        public int TimeSlotId { get; set; }
        public int PricingId { get; set; }
        public int GroupConditionId { get; set; }
        public double Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? FinalTestDate { get; set; }
        public int MaxInstances { get; set; }
        public class UpdateGroupDefinitionCommandHandler : IRequestHandler<UpdateGroupDefinitionCommand, Response<int>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
            public UpdateGroupDefinitionCommandHandler(IGroupDefinitionRepositoryAsync GroupDefinitionRepository)
            {
                _GroupDefinitionRepositoryAsync = GroupDefinitionRepository;
            }
            public async Task<Response<int>> Handle(UpdateGroupDefinitionCommand command, CancellationToken cancellationToken)
            {
                var groupDefinition = await _GroupDefinitionRepositoryAsync.GetByIdAsync(command.Id);

                if (groupDefinition == null)
                {
                    throw new ApiException($"Group Not Found.");
                }
                else if (groupDefinition.Status != (int)GroupDefinationStatusEnum.New)
                {
                    throw new ApiException($"Group can't be updated");
                }
                else
                {

                    groupDefinition.SubLevelId = command.SubLevelId;
                    groupDefinition.TimeSlotId = command.TimeSlotId;
                    groupDefinition.PricingId = command.PricingId;
                    groupDefinition.GroupConditionId = command.GroupConditionId;
                    groupDefinition.Discount = command.Discount;
                    groupDefinition.StartDate = command.StartDate;
                    groupDefinition.EndDate = command.EndDate;
                    groupDefinition.FinalTestDate = command.FinalTestDate;
                    groupDefinition.MaxInstances = command.MaxInstances;

                    await _GroupDefinitionRepositoryAsync.UpdateAsync(groupDefinition);
                    return new Response<int>(groupDefinition.Id);
                }
            }
        }
    }
}
