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
        Task<IReadOnlyList<TestInstance>> GetTestInstanceByTeacher(string correctionTeacherId, int? status, int? TestType, int? GroupInstanceId);
        Task<IReadOnlyList<TestInstance>> GetAllTestInstancesResults(int groupInstance);
        int GetAllTestInstancesResultsCount(int groupInstance);
        Task<IReadOnlyList<TestInstance>> GetTestInstanceToActive();
        Task<IReadOnlyList<object>> GetAllClosedAndPendingQuizzAsync(int GroupInstanceId);
        Task<List<TestInstance>> GetTestInstanceByLessonInstanceId(int LessonInstanceId);
        Task<IReadOnlyList<TestInstance>> GetAllTestsToManage(int? GroupDefinitionId, int? GroupInstanceId, int? TestTypeId, int? Status, int PageNumber, int PageSize);
        Task<int> GetAllTestsToManageCount(int? GroupDefinitionId, int? GroupInstanceId, int? TestTypeId, int? Status);
        Task<List<TestInstance>> GetAllTestInstancesByGroup(int groupInstance);
        Task<List<TestInstance>> GetProgressByStudentId(string studentID);
    }
}