using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IGroupConditionDetailsRepositoryAsync : IGenericRepositoryAsync<GroupConditionDetail>
    {
        List<GroupConditionDetail> GetByGroupConditionId(int configCondID);
        List<GroupConditionDetail> GetByGroupConditionIds(List<int> configCondIDs);
    }
}
