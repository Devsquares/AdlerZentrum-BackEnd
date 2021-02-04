using Application.DTOs.GroupConditionPromoCodeModel;
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
        public int NumberOfSlots { get; set; }
        public int NumberOfSlotsWithPlacementTest { get; set; }
        public string Name { get; set; }
        public List<List<GroupConditionPromoCodeInputModel>> PromoCodes { get; set; }

        public class CreateGroupConditionCommandHandler : IRequestHandler<CreateGroupConditionCommand, Response<int>>
        {
            private readonly IGroupConditionRepositoryAsync _groupConditionRepository;
            private readonly IGroupConditionDetailsRepositoryAsync _groupConditionDetailsRepository;
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepository;
            public CreateGroupConditionCommandHandler(IGroupConditionRepositoryAsync groupConditionRepository,
                IGroupConditionDetailsRepositoryAsync groupConditionDetailsRepository,
                IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepository)
            {
                _groupConditionRepository = groupConditionRepository;
                _groupConditionDetailsRepository = groupConditionDetailsRepository;
                _groupConditionPromoCodeRepository = groupConditionPromoCodeRepository;
            }
            public async Task<Response<int>> Handle(CreateGroupConditionCommand command, CancellationToken cancellationToken)
            {
                var groupCondition = new Domain.Entities.GroupCondition();

                Reflection.CopyProperties(command, groupCondition);
                await _groupConditionRepository.AddAsync(groupCondition);
                CreateDependenciesGroupCondition _createDependenciesGroupCondition = new CreateDependenciesGroupCondition(_groupConditionDetailsRepository, _groupConditionPromoCodeRepository);
                _createDependenciesGroupCondition.Create(groupCondition.Id, command.PromoCodes);
                return new Response<int>(groupCondition.Id);

            }
        }
    }
}
