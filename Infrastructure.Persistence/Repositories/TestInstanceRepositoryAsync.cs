using Application.Enums;
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
                  x.Test.TestTypeId == (int)testType).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<TestInstance>> GetTestInstanceToAssgin()
        {
            return await _testInstances
            .Include(x => x.Test)
            .Include(x => x.Student)
            .Include(x => x.LessonInstance)
            .ThenInclude(x => x.GroupInstance)
             .Where(x => x.CorrectionTeacherId == null).ToListAsync();
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

        public virtual async Task<IReadOnlyList<TestInstance>> GetAllTestsToManage()
        {
            var x = await _testInstances
            .Include(x => x.Test)
            .Include(x => x.Student)
            .Include(x => x.LessonInstance)
            .ThenInclude(x => x.GroupInstance)
            .ThenInclude(x => x.GroupDefinition)
            .Where(x => x.Status == (int)TestInstanceEnum.Closed || x.Status == (int)TestInstanceEnum.Pending)
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
            return x;
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
            return await _testInstances.Where(x => x.LessonInstance.GroupInstanceId == groupInstance).ToListAsync();
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

        public async Task<IReadOnlyList<TestInstance>> GetTestInstanceByTeacher(string correctionTeacherId, int? status, int? TestType)
        {
            var query = _testInstances
              .Include(x => x.Test)
              .Include(x => x.Student)
              .Include(x => x.LessonInstance)
              .ThenInclude(x => x.GroupInstance);

            if (status != null)
            {
                query.Where(x => x.Status == status);
            }

            if (TestType != null)
            {
                query.Where(x => x.Test.TestTypeId == TestType);
            }

            return await query.Where(x => x.CorrectionTeacherId == correctionTeacherId).ToListAsync();
        }

        public async Task<List<TestInstance>> GetProgressByStudentId(string studentID)
        {
            return await _testInstances
                .Include(x => x.Test)
                .Include(x => x.Test.Level)
                .Include(x => x.Test.Sublevel)
                .Where(x => x.StudentId == studentID).ToListAsync();
        }
    }
}

