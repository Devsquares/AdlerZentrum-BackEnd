using Application.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Features;

namespace Application.Interfaces.Repositories
{
    public interface ITestInstanceRepositoryAsync : IGenericRepositoryAsync<TestInstance>
    {
        Task<IReadOnlyList<TestInstance>> GetAllTestsForStudentAsync(string student, int groupInstance, TestTypeEnum testType);
        IReadOnlyList<TestInstance> GetTestInstanceToAssgin(string studentName, string testName, int? testType, bool assigend, int? groupInsatanceId, int? testInstanceId, int pageNumber, int pageSize, out int count);
        IReadOnlyList<LessonInstance> GetLessonInstanceToAssign(int? groupInsatanceId, int pageNumber, int pageSize, out int count);
        IReadOnlyList<GroupInstance> GetGroupInstanceToAssign(int? sublevelId, int? groupDefinitionId, int pageNumber, int pageSize, out int count);
        Task<IReadOnlyList<TestInstance>> GetTestInstanceByTeacher(string correctionTeacherId, int? status, int? TestType, int? GroupInstanceId);
        Task<IReadOnlyList<TestInstance>> GetAllTestInstancesResults(int groupInstance);
        int GetAllTestInstancesResultsCount(int groupInstance);
        Task<IReadOnlyList<TestInstance>> GetTestInstanceToActive();
        Task<IReadOnlyList<object>> GetAllClosedAndPendingQuizzAsync(int GroupInstanceId);
        Task<List<TestInstance>> GetTestInstanceByLessonInstanceId(int LessonInstanceId);
        Task<IReadOnlyList<TestInstance>> GetAllTestsToManage(int? GroupDefinitionId, int? GroupInstanceId, int? TestTypeId, int? Status, bool? reCorrectionRequest, int PageNumber, int PageSize);
        Task<int> GetAllTestsToManageCount(int? GroupDefinitionId, int? GroupInstanceId, int? TestTypeId, int? Status, bool? reCorrectionRequest);
        Task<List<TestInstance>> GetAllTestInstancesByGroup(int groupInstance);
        Task<List<TestInstance>> GetProgressByStudentId(string studentID, List<int> groupInstanceIds);
        Task<List<TestInstance>> GetAllTestInstancesByGroupAndTest(int groupInstance, int testId);
        Task<List<TestInstance>> GetAllPlacementTestsByStudent(string studentId);
        Task<List<LateSubmissionsViewModel>> GetLateSubmissions(string TeacherName, int pageNumber, int pageSize, bool DelaySeen);
        int GetLateSubmissionsCount(string TeacherName, bool DelaySeen);
        Test GetSubLevelTestByGroupInstance(int groupinstanceId);
        Test GetFinalLevelTestByGroupInstance(int groupinstanceId);
        Test GetQuizTestByGroupInstanceByLessonDef(int groupinstanceId, int lessonDefinationdId);
        Task<List<TestInstance>> GetAllTestInstancesByListGroup(List<int> groupInstanceIds);
        Task<IReadOnlyList<TestInstance>> GetFeedbackSheetInstancesForStudentByGroupInstanceId(string StudentId, int GroupInstanceId);
        Task<IReadOnlyList<TestInstance>> GetFeedbackSheetInstancesForStudent(string StudentId);
        Task<IReadOnlyList<TestInstance>> GetFeedbackSheetsPagedReponseAsync(int pageNumber, int pageSize, int GroupInstanceId, string StudentName, TestInstanceEnum Status, int LessonInstanceId);
        Task<int> GetFeedbackSheetsPagedReponseCountAsync(int GroupInstanceId, string StudentName, TestInstanceEnum Status, int LessonInstanceId);
    }
}