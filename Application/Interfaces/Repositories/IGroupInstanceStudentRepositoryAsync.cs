using Domain.Entities;
using Domain.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGroupInstanceStudentRepositoryAsync : IGenericRepositoryAsync<GroupInstanceStudents>
    {
        int GetCountOfStudents(int groupId);

        List<string> GetEmailsByGroupDefinationId(int groupDefinationId);
        GroupInstanceStudents GetByStudentId(string studentId, int groupId);
        Task<int> GetCountOfPlacmentTestStudents(int groupId);
        Task<int> GetCountOfStudentsByGroupDefinitionId(int groupDefinitionId);
        GroupInstanceModel GetLastByStudentId(string studentId);
        List<GroupInstanceModel> GetAllLastByStudentId(string studentId);
        Task<List<ApplicationUser>> GetAllStudentInGroupInstanceByStudentId(string studentId);
        Task<List<ApplicationUser>> GetAllStudentInGroupDefinitionByStudentId(string studentId);

        List<GroupInstanceStudents> SaveAllGroupInstanceStudents(int groupDefinitionId, List<StudentsGroupInstanceModel> groupInstanceStudentslist, out List<GroupInstanceStudents> groupInstanceStudentsobjectList);
        List<IGrouping<int, GroupInstanceStudents>> GetAllByGroupDefinition(int? groupDefinitionId= null, int? groupInstanceId = null);
        List<GroupInstanceStudents> GetByGroupDefinitionAndGroupInstance(int groupDefinitionId, int? groupinstanceId = null);
    }
}
