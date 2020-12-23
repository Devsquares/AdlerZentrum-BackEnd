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
        public int? Status { get; set; }
        public int NumberOfSolts { get; set; }
        public int NumberOfSlotsWithPlacementTest { get; set; }

        public class CreateGroupDefinitionCommandHandler : IRequestHandler<CreateGroupDefinitionCommand, Response<int>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            public CreateGroupDefinitionCommandHandler(IGroupDefinitionRepositoryAsync GroupDefinitionRepository)
            {
                _GroupDefinitionRepository = GroupDefinitionRepository;
            }
            public async Task<Response<int>> Handle(CreateGroupDefinitionCommand command, CancellationToken cancellationToken)
            {
                var GroupDefinition = new Domain.Entities.GroupDefinition();

                Reflection.CopyProperties(command, GroupDefinition);
                await _GroupDefinitionRepository.AddAsync(GroupDefinition);
                return new Response<int>(GroupDefinition.Id);

            }
        }
    }
}
