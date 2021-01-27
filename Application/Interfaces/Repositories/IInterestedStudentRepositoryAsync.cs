using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IInterestedStudentRepositoryAsync : IGenericRepositoryAsync<InterestedStudent>
    {
        InterestedStudent GetByStudentId(string studentId, int groupDefinitionId);
        object GetByGroupDefinitionId(int groupDefinitionId);
        int GetCountOfStudentsByGroupDefinitionId(int groupDefinitionId);
    }
}
