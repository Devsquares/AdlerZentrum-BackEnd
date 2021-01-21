using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IGroupConditionPromoCodeRepositoryAsync : IGenericRepositoryAsync<GroupConditionPromoCode>
    {
        List<GroupConditionPromoCode> GetByGroupConditionDetailId(List<GroupConditionDetail> groupConditionDetails);
        bool CheckPromoCodeCountInGroupInstance(int groupInstanceId, int promocodeId);
    }
}
