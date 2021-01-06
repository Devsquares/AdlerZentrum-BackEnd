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
    }
}
