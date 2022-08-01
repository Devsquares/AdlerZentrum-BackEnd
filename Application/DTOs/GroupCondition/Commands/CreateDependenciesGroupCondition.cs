using Application.DTOs.GroupConditionPromoCodeModel;
using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateDependenciesGroupCondition
    {
        private readonly IGroupConditionDetailsRepositoryAsync _groupConditionDetailsRepository;
        private readonly IGroupConditionPromoCodeRepositoryAsync _groupConditionPromoCodeRepository;
        public CreateDependenciesGroupCondition(
               IGroupConditionDetailsRepositoryAsync groupConditionDetailsRepository,
               IGroupConditionPromoCodeRepositoryAsync groupConditionPromoCodeRepository)
        {
            _groupConditionDetailsRepository = groupConditionDetailsRepository;
            _groupConditionPromoCodeRepository = groupConditionPromoCodeRepository;
        }
        public async Task Create(int groupConditionId, List<List<GroupConditionPromoCodeInputModel>> PromoCodes)
        {
            // add details
            List<GroupConditionDetail> groupConditionDetailsList = new List<GroupConditionDetail>();
            for (int i = 0; i < PromoCodes.Count; i++)
            {
                groupConditionDetailsList.Add(new GroupConditionDetail() { GroupConditionId = groupConditionId });
            }
            await _groupConditionDetailsRepository.AddBulkAsync(groupConditionDetailsList);
            // add promocode for group condition
            List<GroupConditionPromoCode> groupConditionPromocodesList = new List<GroupConditionPromoCode>();
            for (int i = 0; i < PromoCodes.Count; i++)
            {
                foreach (var promoCode in PromoCodes[i])
                {
                    groupConditionPromocodesList.Add(
                        new GroupConditionPromoCode()
                        {
                            GroupConditionDetailsId = groupConditionDetailsList[i].Id,
                            PromoCodeId = promoCode.PromoCodeId,
                            Count = promoCode.Count
                        }
                    );
                }

            }
            await _groupConditionPromoCodeRepository.AddBulkAsync(groupConditionPromocodesList);
        }
    }
}
