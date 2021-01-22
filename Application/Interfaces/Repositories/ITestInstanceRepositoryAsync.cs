using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITestInstanceRepositoryAsync : IGenericRepositoryAsync<TestInstance>
    {
        Task<IReadOnlyList<TestInstance>> GetAllQuizzForStudentAsync(string student, int groupInstance);
        Task<IReadOnlyList<TestInstance>> GetTestInstanceToAssgin();
        Task<IReadOnlyList<TestInstance>> GetAllTestInstancesResults(int groupInstance);
        int GetAllTestInstancesResultsCount(int groupInstance);
        Task<IReadOnlyList<object>> GetAllClosedAndPendingQuizzAsync(int GroupInstanceId);
        Task<List<TestInstance>> GetTestInstanceByLessonInstanceId(int LessonInstanceId);
    }
} 