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
using System.Transactions;

namespace Application.DTOs
{
    public class UpdateGroupConditionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int? Status { get; set; }
        public int NumberOfSlots { get; set; }
        public int NumberOfSlotsWithPlacementTest { get; set; }
        public List<List<GroupConditionPromoCodeInputModel>> PromoCodes { get; set; }
        public string Name { get; set; }

        public class UpdateGroupConditionCommandHandler : IRequestHandler<UpdateGroupConditionCommand, Response<int>>
        {
            private readonly IGroupConditionRepositoryAsync _GroupConditionRepositoryAsync;
            private readonly IGroupConditionDetailsRepositoryAsync _groupConditionDetailsRepository;
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepository;
            public UpdateGroupConditionCommandHandler(IGroupConditionRepositoryAsync GroupConditionRepository,
                 IGroupConditionDetailsRepositoryAsync groupConditionDetailsRepository,
                IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepository)
            {
                _GroupConditionRepositoryAsync = GroupConditionRepository;
                _groupConditionDetailsRepository = groupConditionDetailsRepository;
                _groupConditionPromoCodeRepository = groupConditionPromoCodeRepository;
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
                    //delete group condition promocodes and details
                    var deletedGroupConditionDetail = _groupConditionDetailsRepository.GetByGroupConditionId(command.Id);
                    var deletedPromocodes = _groupConditionPromoCodeRepository.GetByGroupConditionDetailId(deletedGroupConditionDetail);
                    foreach (var item in deletedPromocodes)
                    {
                        item.GroupConditionDetails = null;
                        item.PromoCode = null;
                    }
                    await _groupConditionPromoCodeRepository.DeleteBulkAsync(deletedPromocodes);
                    await _groupConditionDetailsRepository.DeleteBulkAsync(deletedGroupConditionDetail);
                    // add new ones
                    CreateDependenciesGroupCondition _createDependenciesGroupCondition = new CreateDependenciesGroupCondition(_groupConditionDetailsRepository, _groupConditionPromoCodeRepository);
                    await _createDependenciesGroupCondition.Create(GroupCondition.Id, command.PromoCodes);
                    await _GroupConditionRepositoryAsync.UpdateAsync(GroupCondition);
                    return new Response<int>(GroupCondition.Id);
                }
            }
        }
    }
}
