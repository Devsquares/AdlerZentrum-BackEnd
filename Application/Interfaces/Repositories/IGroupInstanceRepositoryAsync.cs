using Domain.Entities;
using System.Collections;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface IGroupInstanceRepositoryAsync : IGenericRepositoryAsync<GroupInstance>
    {
        IReadOnlyList<GroupInstanceStudents> GetStudents(int groupId);
        int? GetActiveGroupInstance(string userId);
    }
}
