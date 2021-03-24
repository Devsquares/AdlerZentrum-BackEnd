using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IStudentInfoRepositoryAsync : IGenericRepositoryAsync<StudentInfo>
    {
        StudentInfo GetUserById(string Id);
         bool CheckBySublevel(string studentId, int SublevelId);
    }
}
