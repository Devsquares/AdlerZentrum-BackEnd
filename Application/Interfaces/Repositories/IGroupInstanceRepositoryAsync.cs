using Domain.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGroupInstanceRepositoryAsync : IGenericRepositoryAsync<GroupInstance>
    {
        IReadOnlyList<GroupInstanceStudents> GetStudents(int groupId);
        int? GetActiveGroupInstance(string userId);
        void AddStudentToTheGroupInstance(int groupId, string studentId);
        Task<GroupInstance> GetByIdAsync(int id);
        GroupInstance GetByGroupDefinitionId(int groupdefinitionid);
    }
}
