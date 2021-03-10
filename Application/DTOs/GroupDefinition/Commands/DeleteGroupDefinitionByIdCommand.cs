using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;

namespace Application.DTOs
{
    public class DeleteGroupDefinitionByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteGroupDefinitionByIdCommandHandler : IRequestHandler<DeleteGroupDefinitionByIdCommand, Response<int>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            public DeleteGroupDefinitionByIdCommandHandler(IGroupDefinitionRepositoryAsync GroupDefinition)
            {
                _GroupDefinitionRepository = GroupDefinition;
            }
            public async Task<Response<int>> Handle(DeleteGroupDefinitionByIdCommand command, CancellationToken cancellationToken)
            {
                var groupDefinition = await _GroupDefinitionRepository.GetByIdAsync(command.Id);
                if (groupDefinition == null) throw new ApiException($"Group Not Found.");
                if (groupDefinition.Status != (int)GroupDefinationStatusEnum.New) throw new ApiException($"Group Not Allowed to update.");
                
                await _GroupDefinitionRepository.DeleteAsync(groupDefinition);
                return new Response<int>(groupDefinition.Id);
            }
        }
    }
}
