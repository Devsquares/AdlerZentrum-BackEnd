using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IHomeWorkSubmitionRepositoryAsync : IGenericRepositoryAsync<HomeWorkSubmition>
    {
        Task<IReadOnlyList<HomeWorkSubmition>> GetAllForStudentAsync(string studentId, int groupInstanceId);
        Task<IReadOnlyList<HomeWorkSubmition>> GetAllSolvedAsync();
        Task<IReadOnlyList<HomeWorkSubmition>> GetAllByGroupInstanceAsync(int groupInstanceId);
    }
}
