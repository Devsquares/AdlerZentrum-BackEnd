using Application.Filters;
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
        Task AddStudentToTheGroupInstance(int groupId, string studentId);
        new Task<GroupInstance> GetByIdAsync(int id);
        GroupInstance GetByGroupDefinitionId(int groupDefinitionId);
        List<StudentsGroupInstanceModel> GetListByGroupDefinitionId(int groupDefinitionId, List<int> groupInstancelist = null);
        int GetCountByGroupDefinitionId(int groupDefinitionId);
        Task<GroupInstance> GetByIdPendingorCompleteAsync(int id);
        Task<List<GroupInstance>> GetByGroupDefinitionAndGroupInstanceAsync(int groupDefinitionId, int? groupinstanceId = null);
        Task<List<GroupInstance>> GetByGroupDefinitionAndGroupInstanceWithoutSublevelAsync(int groupDefinitionId, int? groupinstanceId = null);
        
        IReadOnlyList<GroupInstance> GetPagedGroupInstanceReponseAsync(FilteredRequestParameter filteredRequestParameter, List<int> status, out int count);
        int? IsOtherActiveGroupInTheGroupDef(int groupDefinitionId);
    }
}
