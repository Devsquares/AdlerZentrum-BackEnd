using Domain.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGroupInstanceStudentRepositoryAsync : IGenericRepositoryAsync<GroupInstanceStudents>
    {
        int GetCountOfStudents(int groupId);

        List<string> GetEmailsByGroupDefinationId(int groupDefinationId);
        GroupInstanceStudents GetByStudentId(string studentId, int groupId);
        int GetCountOfPlacmentTestStudents(int groupId);
        int GetCountOfStudentsByGroupDefinitionId(int groupDefinitionId);
        GroupInstance GetLastByStudentId(string studentId);
        List<GroupInstance> GetAllLastByStudentId(string studentId);
        List<ApplicationUser> GetAllStudentInGroupInstanceByStudentId(string studentId);
        List<ApplicationUser> GetAllStudentInGroupDefinitionByStudentId(string studentId);
    }
}
