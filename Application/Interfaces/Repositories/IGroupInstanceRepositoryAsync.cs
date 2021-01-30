using Domain.Entities;
using Domain.Models;
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
        GroupInstance GetByGroupDefinitionId(int groupDefinitionId);
        List<StudentsGroupInstanceModel> GetListByGroupDefinitionId(int groupDefinitionId, List<int> groupInstancelist = null);
        int GetCountByGroupDefinitionId(int groupDefinitionId);
        Task<GroupInstance> GetByIdPendingorCompleteAsync(int id);
    }
}
