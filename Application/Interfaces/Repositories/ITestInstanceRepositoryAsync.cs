using Application.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITestInstanceRepositoryAsync : IGenericRepositoryAsync<TestInstance>
    {
        Task<IReadOnlyList<TestInstance>> GetAllTestsForStudentAsync(string student, int groupInstance, TestTypeEnum testType);
        Task<IReadOnlyList<TestInstance>> GetTestInstanceToAssgin();
        Task<IReadOnlyList<TestInstance>> GetTestInstanceToActive();
    }
}
