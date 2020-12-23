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
    public class CreateGroupConditionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int? Status { get; set; }
        public int NumberOfSolts { get; set; }
        public int NumberOfSlotsWithPlacementTest { get; set; }

        public class CreateGroupConditionCommandHandler : IRequestHandler<CreateGroupConditionCommand, Response<int>>
        {
            private readonly IGroupConditionRepositoryAsync _groupConditionRepository;
            public CreateGroupConditionCommandHandler(IGroupConditionRepositoryAsync groupConditionRepository)
            {
                _groupConditionRepository = groupConditionRepository;
            }
            public async Task<Response<int>> Handle(CreateGroupConditionCommand command, CancellationToken cancellationToken)
            {
                var groupCondition = new Domain.Entities.GroupCondition();

                Reflection.CopyProperties(command, groupCondition);
                await _groupConditionRepository.AddAsync(groupCondition);
                return new Response<int>(groupCondition.Id);

            }
        }
    }
}
