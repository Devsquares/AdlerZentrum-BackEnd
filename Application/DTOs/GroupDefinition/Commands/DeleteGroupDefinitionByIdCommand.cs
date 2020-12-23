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
                var groupInstance = await _GroupDefinitionRepository.GetByIdAsync(command.Id);
                if (groupInstance == null) throw new ApiException($"Group Not Found.");
                await _GroupDefinitionRepository.DeleteAsync(groupInstance);
                return new Response<int>(groupInstance.Id);
            }
        }
    }
}
