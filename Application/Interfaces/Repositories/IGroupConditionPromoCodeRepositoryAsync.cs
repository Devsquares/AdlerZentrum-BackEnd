using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IGroupConditionPromoCodeRepositoryAsync : IGenericRepositoryAsync<GroupConditionPromoCode>
    {
        List<GroupConditionPromoCode> GetByGroupConditionDetailId(List<GroupConditionDetail> groupConditionDetails);
        bool CheckPromoCodeCountInGroupInstance(int groupInstanceId, int promocodeInstanceId, List<GroupInstanceStudents> groupInstanceStudentsList = null, string promoCodeKey = null, bool isAutomaticCreate = false);
        bool CheckPromoCodeInGroupDefinitionGeneral(int groupDefinitionId, int promocodeId, string promoCodeKey = null, bool isAutomaticCreate = false);
        List<IGrouping<int, GroupConditionPromoCode>> GetAllByGroupCondition(int groupConditionId);
        bool CheckStudentPromoCodeInstance(string email, string studentId, int promocodeInstanceId);
    }
}
