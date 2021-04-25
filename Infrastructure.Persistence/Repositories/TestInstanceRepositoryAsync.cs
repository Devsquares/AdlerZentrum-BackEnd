using Application.Enums;
using Application.Features;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class TestInstanceRepositoryAsync : GenericRepositoryAsync<TestInstance>, ITestInstanceRepositoryAsync
    {
        private readonly DbSet<TestInstance> _testInstances;

        public TestInstanceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _testInstances = dbContext.Set<TestInstance>();
        }

        public virtual async Task<IReadOnlyList<TestInstance>> GetAllTestsForStudentAsync(string student, int groupInstance, TestTypeEnum testType)
        {
            return await _testInstances
                  .Include(x => x.LessonInstance)
                  .Include(x => x.Test)
                  .Include(x => x.Test.Questions)
                  .ThenInclude(x => x.SingleQuestions)
                  .ThenInclude(x => x.Choices)
                  .Where(x => x.LessonInstance.GroupInstanceId == groupInstance &&
                  x.StudentId == student &&
                  x.Test.TestTypeId == (int)testType)
                  .OrderBy(x => x.LessonInstanceId).ToListAsync();
        }

        public virtual IReadOnlyList<TestInstance> GetTestInstanceToAssgin(string studentName, string testName, int? testType, bool assigend, int? groupInsatanceId, int? testInstanceId, int pageNumber, int pageSize, out int count)
        {
            var query = _testInstances.AsQueryable();

            if (!string.IsNullOrWhiteSpace(studentName))
            {
                query = query.Where(x => x.Student.FirstName.Contains(studentName) || x.Student.LastName.Contains(studentName));
            }

            if (!string.IsNullOrWhiteSpace(testName))
            {
                query = query.Where(x => x.Test.Name.Contains(testName) || x.Test.Name.Contains(testName));
            }

            if (testType != null)
            {
                query = query.Where(x => x.Test.TestTypeId == testType);
            }

            if (groupInsatanceId != null)
            {
                query = query.Where(x => x.GroupInstanceId == groupInsatanceId);
            }

            if (testInstanceId != null)
            {
                query = query.Where(x => x.LessonInstanceId == testInstanceId);
            }

            if (assigend)
            {
                query = query.Where(x => x.CorrectionTeacherId != null);
            }
            else
            {
                query = query.Where(x => x.CorrectionTeacherId == null);
            }
            query = query.Where(x => x.Status <= (int)TestInstanceEnum.Solved);
            count = query.Count();
            return query
                .Include(x => x.Test)
                .Include(x => x.CorrectionTeacher)
                .Include(x => x.GroupInstance)
                .Include(x => x.LessonInstance)
                .ThenInclude(x => x.GroupInstance)
                .Include(x => x.Student)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public virtual IReadOnlyList<LessonInstance> GetLessonInstanceToAssign(int? groupInsatanceId, int pageNumber, int pageSize, out int count)
        {
            var query = _testInstances.AsQueryable();
            if (groupInsatanceId != null)
            {
                query = query.Where(x => x.GroupInstanceId == groupInsatanceId);
            }
            query = query.Where(x => x.CorrectionTeacherId == null);

            query = query.Where(x => x.Status <= (int)TestInstanceEnum.Solved);
            count = query.Count();
            return query
                .Include(x => x.Test)
                .Include(x => x.CorrectionTeacher)
                .Include(x => x.GroupInstance)
                .Include(x => x.LessonInstance)
                .ThenInclude(x => x.GroupInstance)
                .Include(x => x.Student)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => x.LessonInstance).Distinct().ToList();
        }

        public virtual IReadOnlyList<GroupInstance> GetGroupInstanceToAssign(int? sublevelId, int? groupDefinitionId, int pageNumber, int pageSize, out int count)
        {
            var query = _testInstances.AsQueryable();
            if (groupDefinitionId != null)
            {
                query = query.Where(x => x.GroupInstance.GroupDefinitionId == groupDefinitionId);
            }
            if (sublevelId != null)
            {
                query = query.Where(x => x.GroupInstance.GroupDefinition.SubLevelId == sublevelId);
            }
            query = query.Where(x => x.CorrectionTeacherId == null);

            query = query.Where(x => x.Status <= (int)TestInstanceEnum.Solved);
            count = query.Count();
            return query
                .Include(x => x.Test)
                .Include(x => x.CorrectionTeacher)
                .Include(x => x.GroupInstance)
                .Include(x => x.LessonInstance)
                .ThenInclude(x => x.GroupInstance)
                .Include(x => x.Student)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => x.GroupInstance).Distinct().ToList();
        }


        public Test GetSubLevelTestByGroupInstance(int groupinstanceId)
        {
            return _testInstances.Include(x => x.Test).Where(x => x.GroupInstanceId == groupinstanceId && x.Test.TestTypeId == (int)TestTypeEnum.subLevel).FirstOrDefault()?.Test;
        }

        public Test GetFinalLevelTestByGroupInstance(int groupinstanceId)
        {
            return _testInstances.Include(x => x.Test).Where(x => x.GroupInstanceId == groupinstanceId && x.Test.TestTypeId == (int)TestTypeEnum.final).FirstOrDefault()?.Test;
        }

        public Test GetQuizTestByGroupInstanceByLessonDef(int groupinstanceId, int lessonDefinationdId)
        {
            return _testInstances
            .Include(x => x.Test)
            .Include(x => x.LessonInstance)
            .Where(x => x.GroupInstanceId == groupinstanceId
             && x.Test.TestTypeId == (int)TestTypeEnum.final
             && x.LessonInstance.LessonDefinitionId == lessonDefinationdId)
            .FirstOrDefault()?.Test;
        }

        public virtual async Task<IReadOnlyList<TestInstance>> GetTestInstanceToActive()
        {
            return await _testInstances
            .Include(x => x.Test)
            .Include(x => x.Student)
            .Include(x => x.LessonInstance)
            .ThenInclude(x => x.GroupInstance)
             .Where(x => x.Status == (int)TestInstanceEnum.Closed).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<TestInstance>> GetAllTestsToManage(int? GroupDefinitionId, int? GroupInstanceId, int? TestTypeId, int? Status, bool? reCorrectionRequest, int pageNumber, int pageSize)
        {
            var query = _testInstances.AsQueryable();
            if (GroupDefinitionId != null)
            {
                query = query.Where(x => x.GroupInstance.GroupDefinitionId == GroupDefinitionId);
            }
            if (GroupInstanceId != null)
            {
                query = query.Where(x => x.GroupInstanceId == GroupInstanceId);
            }
            if (TestTypeId != null)
            {
                query = query.Where(x => x.Test.TestTypeId == TestTypeId);
            }
            if (Status != null)
            {
                query = query.Where(x => x.Status == Status);
            }
            if (reCorrectionRequest != null)
            {
                query = query.Where(x => x.ReCorrectionRequest == reCorrectionRequest);
            }
            return await query
           .Include(x => x.Test)
           .Include(x => x.Student)
           .Include(x => x.LessonInstance)
           .ThenInclude(x => x.GroupInstance)
           .ThenInclude(x => x.GroupDefinition)
           .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
           //    .Where(x => x.Status == (int)TestInstanceEnum.Closed || x.Status == (int)TestInstanceEnum.Pending)
           .ToListAsync();

            // var queryNumericRange =
            //     from test in _testInstances
            //     group new AllTestsToManageViewModel()
            //     {
            //         GroupInstanceId = test.GroupInstanceId.Value,
            //         Status = test.Status,
            //         TestName = test.Test.Name,
            //         TestId = test.Test.Id
            //     } by new { test.GroupInstanceId, test.TestId } into percentGroup
            //     orderby percentGroup.Key
            //     select percentGroup;


            // var items = queryNumericRange.ToList();
        }

        public virtual async Task<int> GetAllTestsToManageCount(int? GroupDefinitionId, int? GroupInstanceId, int? TestTypeId, int? Status, bool? reCorrectionRequest)
        {
            var query = _testInstances.AsQueryable();
            if (GroupDefinitionId != null)
            {
                query = query.Where(x => x.GroupInstance.GroupDefinitionId == GroupDefinitionId);
            }
            if (GroupInstanceId != null)
            {
                query = query.Where(x => x.GroupInstanceId == GroupInstanceId);
            }
            if (TestTypeId != null)
            {
                query = query.Where(x => x.Test.TestTypeId == TestTypeId);
            }
            if (Status != null)
            {
                query = query.Where(x => x.Status == Status);
            }
            if (reCorrectionRequest != null)
            {
                query = query.Where(x => x.ReCorrectionRequest == reCorrectionRequest);
            }
            return query
           .Include(x => x.Test)
           .Include(x => x.Student)
           .Include(x => x.LessonInstance)
           .ThenInclude(x => x.GroupInstance)
           .ThenInclude(x => x.GroupDefinition)
           //    .Where(x => x.Status == (int)TestInstanceEnum.Closed || x.Status == (int)TestInstanceEnum.Pending)
           .Count();

            // var queryNumericRange =
            //     from test in _testInstances
            //     group new AllTestsToManageViewModel()
            //     {
            //         GroupInstanceId = test.GroupInstanceId.Value,
            //         Status = test.Status,
            //         TestName = test.Test.Name,
            //         TestId = test.Test.Id
            //     } by new { test.GroupInstanceId, test.TestId } into percentGroup
            //     orderby percentGroup.Key
            //     select percentGroup;


            // var items = queryNumericRange.ToList();
        }

        public override Task<TestInstance> GetByIdAsync(int id)
        {
            return _testInstances
                   .Include(x => x.Test)
                   .ThenInclude(x => x.Questions)
                   .ThenInclude(x => x.SingleQuestions)
                   .ThenInclude(x => x.Choices)
                   .Include(x => x.Student)
                   .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<IReadOnlyList<TestInstance>> GetAllTestInstancesResults(int groupInstance)
        {
            return await _testInstances
                  .Include(x => x.Student)
                  .Include(x => x.LessonInstance)
                  .ThenInclude(x => x.GroupInstance)
                  .Where(x => x.LessonInstance.GroupInstanceId == groupInstance && x.Status == (int)TestInstanceEnum.Corrected).ToListAsync();
        }

        public virtual int GetAllTestInstancesResultsCount(int groupInstance)
        {
            return _testInstances
                .Include(x => x.Student)
                .Include(x => x.LessonInstance)
                .ThenInclude(x => x.GroupInstance)
                .Where(x => x.LessonInstance.GroupInstanceId == groupInstance && x.Status == (int)TestInstanceEnum.Corrected).Count();
        }

        public async Task<List<TestInstance>> GetAllTestInstancesByGroup(int groupInstance)
        {
            return await _testInstances.Include(x => x.Test).Where(x => x.LessonInstance.GroupInstanceId == groupInstance).ToListAsync();
        }

        public async Task<List<TestInstance>> GetAllTestInstancesByGroupAndTest(int groupInstance, int testId)
        {
            return await _testInstances.Where(x => x.LessonInstance.GroupInstanceId == groupInstance && x.TestId == testId).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<object>> GetAllClosedAndPendingQuizzAsync(int GroupInstanceId)
        {
            return await _testInstances
                  .Include(x => x.LessonInstance)
                  .Where(x => (x.Status == (int)TestInstanceEnum.Closed || x.Status == (int)TestInstanceEnum.Pending) && x.Test.TestTypeId == (int)TestTypeEnum.quizz).Where(x => x.LessonInstance.GroupInstanceId == GroupInstanceId).Select(x => x.LessonInstance).ToListAsync();
        }


        public virtual async Task<List<TestInstance>> GetTestInstanceByLessonInstanceId(int LessonInstanceId)
        {
            return await _testInstances.Where(x => x.LessonInstanceId == LessonInstanceId).ToListAsync();
        }

        public async Task<IReadOnlyList<TestInstance>> GetTestInstanceByTeacher(string correctionTeacherId, int? status, int? TestType, int? GroupInstanceId)
        {
            var query = _testInstances.AsQueryable();

            if (status != null)
            {
                query = query.Where(x => x.Status == status);
            }

            if (TestType != null)
            {
                query = query.Where(x => x.Test.TestTypeId == TestType);
            }
            if (GroupInstanceId != null)
            {
                query = query.Where(x => x.GroupInstanceId == GroupInstanceId);
            }

            return await query.Where(x => x.CorrectionTeacherId == correctionTeacherId)
              .Include(x => x.Test)
              .Include(x => x.Student)
              .Include(x => x.LessonInstance)
              .ThenInclude(x => x.GroupInstance).ToListAsync();
        }

        public async Task<List<TestInstance>> GetProgressByStudentId(string studentID, List<int> groupInstanceIds)
        {
            return await _testInstances
                .Include(x => x.Test)
                .Include(x => x.Test.Level)
                .Include(x => x.Test.Sublevel)
                .Where(x => x.StudentId == studentID && groupInstanceIds.Contains(x.GroupInstanceId.Value)).ToListAsync();
        }

        public async Task<List<TestInstance>> GetAllPlacementTestsByStudent(string studentId)
        {
            return await _testInstances
                 .Include(x => x.LessonInstance)
                 .Include(x => x.Test)
                 .Include(x => x.Test.Questions)
                 .ThenInclude(x => x.SingleQuestions)
                 .ThenInclude(x => x.Choices)
                 .Where(x => x.StudentId == studentId &&
                 x.Test.TestTypeId == (int)TestTypeEnum.placement).ToListAsync();
        }

        public int GetLateSubmissionsCount(string TeacherName, bool DelaySeen)
        {
            return _testInstances.Where(x => x.SubmissionDate == null || x.SubmissionDate > x.CorrectionDueDate
                 && x.ManualCorrection && x.DelaySeen == DelaySeen && String.IsNullOrEmpty(TeacherName) ? true :
         (x.CorrectionTeacher.FirstName.Contains(TeacherName) || x.CorrectionTeacher.LastName.Contains(TeacherName))).Count();
        }

        public async Task<List<LateSubmissionsViewModel>> GetLateSubmissions(string TeacherName, int pageNumber, int pageSize, bool DelaySeen)
        {
            return await _testInstances
                .Include(x => x.GroupInstance)
                .Include(x => x.CorrectionTeacher)
                .Include(x => x.Student)
                .Where(x => x.SubmissionDate == null || x.SubmissionDate == DateTime.MinValue || x.SubmissionDate > x.CorrectionDueDate
                   && x.ManualCorrection && x.DelaySeen == DelaySeen
                   && String.IsNullOrEmpty(TeacherName) ? true :
           (x.CorrectionTeacher.FirstName.Contains(TeacherName) || x.CorrectionTeacher.LastName.Contains(TeacherName)))
             .Select(x => new LateSubmissionsViewModel()
             {
                 Id = x.Id,
                 TestInstance = x,
                 Teacher = x.CorrectionTeacher == null ? string.Empty : x.CorrectionTeacher.FirstName.ToString() + " " + x.CorrectionTeacher.LastName.ToString(),
                 SubmissionDate = x.SubmissionDate,
                 ExpectedDate = x.CorrectionDueDate,
                 DelayDuration = (x.SubmissionDate - x.CorrectionDueDate).Hours,
                 GroupInstance = x.GroupInstance,
                 StudentEmail = x.Student.Email,
                 Test = x.Test,
                 StudentName = x.Student == null ? string.Empty : x.Student.FirstName.ToString() + " " + x.Student.LastName.ToString()
             }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<List<TestInstance>> GetAllTestInstancesByListGroup(List<int> groupInstanceIds)
        {
            return await _testInstances.Include(x => x.Test).Where(x => groupInstanceIds.Contains(x.LessonInstance.GroupInstanceId)).ToListAsync();
        }

        public async Task<IReadOnlyList<TestInstance>> GetFeedbackSheetInstancesForStudentByGroupInstanceId(string StudentId, int GroupInstanceId)
        {
            return await _testInstances.Include(x => x.Test)
            .Where(x => x.StudentId == StudentId && x.GroupInstanceId == GroupInstanceId && x.Test.IsArchived == false && x.Test.TestTypeId == (int)TestTypeEnum.Feedback).ToListAsync();
        }

        public async Task<IReadOnlyList<TestInstance>> GetFeedbackSheetInstancesForStudent(string StudentId)
        {
            return await _testInstances.Include(x => x.Test)
            .Where(x => x.StudentId == StudentId && x.Test.IsArchived == false && x.Test.TestTypeId == (int)TestTypeEnum.Feedback).ToListAsync();
        }

        public async Task<IReadOnlyList<TestInstance>> GetFeedbackSheetsPagedReponseAsync(int pageNumber, int pageSize, int GroupInstanceId, string StudentName, TestInstanceEnum Status, int LessonInstanceId)
        {
            var query = _testInstances.AsQueryable();

            if (GroupInstanceId != 0)
            {
                query = query.Where(x => x.GroupInstanceId == GroupInstanceId);
            }
            if (!String.IsNullOrEmpty(StudentName))
            {
                query = query.Where(x => x.Student.FirstName.Contains(StudentName) || x.Student.LastName.Contains(StudentName));
            }
            if (Status != 0)
            {
                query = query.Where(x => x.Status == (int)Status);
            }
            if (LessonInstanceId != 0)
            {
                query = query.Where(x => x.LessonInstanceId == LessonInstanceId);
            }
            return await query
           .Include(x => x.Test)
           .Include(x => x.Student)
           .Include(x => x.LessonInstance)
           .ThenInclude(x => x.GroupInstance)
           .ThenInclude(x => x.GroupDefinition)
           .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .Where(x => x.Test.TestTypeId == (int)TestTypeEnum.Feedback)
           .ToListAsync();
        }

        public async Task<int> GetFeedbackSheetsPagedReponseCountAsync(int GroupInstanceId, string StudentName, TestInstanceEnum Status, int LessonInstanceId)
        {
            var query = _testInstances.AsQueryable();

            if (GroupInstanceId != 0)
            {
                query = query.Where(x => x.GroupInstanceId == GroupInstanceId);
            }
            if (!String.IsNullOrEmpty(StudentName))
            {
                query = query.Where(x => x.Student.FirstName.Contains(StudentName) || x.Student.LastName.Contains(StudentName));
            }
            if (Status != 0)
            {
                query = query.Where(x => x.Status == (int)Status);
            }
            if (LessonInstanceId != 0)
            {
                query = query.Where(x => x.LessonInstanceId == LessonInstanceId);
            }
            return await query
           .Include(x => x.Test)
           .Include(x => x.Student)
           .Include(x => x.LessonInstance)
           .ThenInclude(x => x.GroupInstance)
           .ThenInclude(x => x.GroupDefinition)
            .AsNoTracking()
            .Where(x => x.Test.TestTypeId == (int)TestTypeEnum.Feedback)
           .CountAsync();
        }
    }
}

