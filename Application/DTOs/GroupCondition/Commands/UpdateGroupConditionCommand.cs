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

namespace Application.DTOs.GroupCondition.Commands
{
    public class UpdateGroupConditionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int? Status { get; set; }
        public int NumberOfSolts { get; set; }
        public int NumberOfSlotsWithPlacementTest { get; set; }

        public class UpdateGroupConditionCommandHandler : IRequestHandler<UpdateGroupConditionCommand, Response<int>>
        {
            private readonly IGroupConditionRepositoryAsync _GroupConditionRepositoryAsync;
            public UpdateGroupConditionCommandHandler(IGroupConditionRepositoryAsync GroupConditionRepository)
            {
                _GroupConditionRepositoryAsync = GroupConditionRepository;
            }
            public async Task<Response<int>> Handle(UpdateGroupConditionCommand command, CancellationToken cancellationToken)
            {
                var GroupCondition = await _GroupConditionRepositoryAsync.GetByIdAsync(command.Id);

                if (GroupCondition == null)
                {
                    throw new ApiException($"Group Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, GroupCondition);
                    await _GroupConditionRepositoryAsync.UpdateAsync(GroupCondition);
                    return new Response<int>(GroupCondition.Id);
                }
            }
        }
    }
}
