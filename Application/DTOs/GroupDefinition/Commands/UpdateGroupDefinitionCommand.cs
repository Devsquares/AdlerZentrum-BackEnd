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
        public int? Status { get; set; }
        public int NumberOfSlots { get; set; }
        public int NumberOfSlotsWithPlacementTest { get; set; }

        public class UpdateGroupDefinitionCommandHandler : IRequestHandler<UpdateGroupDefinitionCommand, Response<int>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
            public UpdateGroupDefinitionCommandHandler(IGroupDefinitionRepositoryAsync GroupDefinitionRepository)
            {
                _GroupDefinitionRepositoryAsync = GroupDefinitionRepository;
            }
            public async Task<Response<int>> Handle(UpdateGroupDefinitionCommand command, CancellationToken cancellationToken)
            {
                var GroupDefinition = await _GroupDefinitionRepositoryAsync.GetByIdAsync(command.Id);

                if (GroupDefinition == null)
                {
                    throw new ApiException($"Group Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, GroupDefinition);
                    await _GroupDefinitionRepositoryAsync.UpdateAsync(GroupDefinition);
                    return new Response<int>(GroupDefinition.Id);
                }
            }
        }
    }
}
