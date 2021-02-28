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
    public class CreateGroupDefinitionCommand : IRequest<Response<int>>
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

        public class CreateGroupDefinitionCommandHandler : IRequestHandler<CreateGroupDefinitionCommand, Response<int>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            public CreateGroupDefinitionCommandHandler(IGroupDefinitionRepositoryAsync GroupDefinitionRepository)
            {
                _GroupDefinitionRepository = GroupDefinitionRepository;
            }
            public async Task<Response<int>> Handle(CreateGroupDefinitionCommand command, CancellationToken cancellationToken)
            {
                var groupDefinition = new Domain.Entities.GroupDefinition();

                Reflection.CopyProperties(command, groupDefinition);
                groupDefinition.Status = (int)GroupDefinationStatusEnum.New;
                await _GroupDefinitionRepository.SetSerialNumberBeforeInsert(groupDefinition);
                await _GroupDefinitionRepository.AddAsync(groupDefinition);
                return new Response<int>(groupDefinition.Id);

            }
        }
    }
}
