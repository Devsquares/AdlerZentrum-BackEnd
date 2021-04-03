using Application.Features;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IHomeWorkSubmitionRepositoryAsync : IGenericRepositoryAsync<HomeWorkSubmition>
    {
        Task<IReadOnlyList<HomeWorkSubmition>> GetAllForStudentAsync(string studentId, int groupInstanceId);
        Task<IReadOnlyList<HomeWorkSubmition>> GetAllByTeacherIdAsync(string TeacherId, int? Status);
        Task<IReadOnlyList<HomeWorkSubmition>> GetAllByGroupInstanceAsync(int groupInstanceId, int? Status);
        Task<List<LateSubmissionsViewModel>> GetLateSubmissions(string TeacherName);
        int GetLateSubmissionsCount(string TeacherName);
    }
}
