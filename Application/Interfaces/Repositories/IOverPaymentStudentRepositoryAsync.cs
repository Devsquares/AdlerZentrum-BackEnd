using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IOverPaymentStudentRepositoryAsync : IGenericRepositoryAsync<OverPaymentStudent>
    {
        OverPaymentStudent GetByStudentId(string studentId, int groupDefinitionId);
        object GetByGroupDefinitionId(int groupDefinitionId);
        int GetCountOfStudentsByGroupDefinitionId(int groupDefinitionId);
        List<OverPaymentStudent> GetDefaultListByGroupDefinitionId(int groupDefinitionId);
        List<OverPaymentStudent> GetPlacementTestListByGroupDefinitionId(int groupDefinitionId);
    }
}
