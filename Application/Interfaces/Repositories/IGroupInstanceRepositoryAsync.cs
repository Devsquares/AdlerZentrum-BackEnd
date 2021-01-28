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
        Task<StudentsGroupInstanceModel> CreateGroupFromInterestedOverPayment(int groupDefinitionId);
        int GetCountByGroupDefinitionId(int groupDefinitionId);
        Task<List<StudentsGroupInstanceModel>> EditGroupByAddStudentFromAnotherGroup(int groupDefinitionId, int srcGroupInstanceId, int desGroupInstanceId, string studentId, int? promoCodeId);
        Task<List<StudentsGroupInstanceModel>> SwapStudentBetweenTwoGroups(int groupDefinitionId, int srcGroupInstanceId, string srcstudentId, int desGroupInstanceId, string desstudentId);
    }
}
