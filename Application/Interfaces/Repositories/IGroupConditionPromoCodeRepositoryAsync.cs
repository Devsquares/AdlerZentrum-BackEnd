using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IGroupConditionPromoCodeRepositoryAsync : IGenericRepositoryAsync<GroupConditionPromoCode>
    {
        List<GroupConditionPromoCode> GetByGroupConditionDetailId(List<GroupConditionDetail> groupConditionDetails);
        bool CheckPromoCodeCountInGroupInstance(int groupInstanceId, int promocodeId, List<GroupInstanceStudents> groupInstanceStudentsList = null, List<StudentsModel> studentsModelList = null);
        bool CheckPromoCodeInGroupDefinitionGeneral(int groupDefinitionId, int promocodeId);
    }
}
