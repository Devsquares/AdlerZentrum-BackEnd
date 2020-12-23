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
    public class DeleteGroupConditionByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteGroupConditionByIdCommandHandler : IRequestHandler<DeleteGroupConditionByIdCommand, Response<int>>
        {
            private readonly IGroupConditionRepositoryAsync _groupConditionRepository;
            public DeleteGroupConditionByIdCommandHandler(IGroupConditionRepositoryAsync groupCondition)
            {
                _groupConditionRepository = groupCondition;
            }
            public async Task<Response<int>> Handle(DeleteGroupConditionByIdCommand command, CancellationToken cancellationToken)
            {
                var groupInstance = await _groupConditionRepository.GetByIdAsync(command.Id);
                if (groupInstance == null) throw new ApiException($"Group Not Found.");
                await _groupConditionRepository.DeleteAsync(groupInstance);
                return new Response<int>(groupInstance.Id);
            }
        }
    }
}
