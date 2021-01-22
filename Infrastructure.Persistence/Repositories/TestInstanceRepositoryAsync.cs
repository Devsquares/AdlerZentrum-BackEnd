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
        private readonly DbSet<TestInstance> _testinstances;
        public TestInstanceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _testinstances = dbContext.Set<TestInstance>();
        }

        public virtual async Task<IReadOnlyList<TestInstance>> GetAllQuizzForStudentAsync(string student, int groupInstance)
        {
            return await _testinstances
                  .Include(x => x.LessonInstance)
                  .Include(x => x.Test)
                  .Include(x => x.Test.Questions)
                  .ThenInclude(x => x.SingleQuestions)
                  .ThenInclude(x => x.Choices)
                  .Where(x => x.LessonInstance.GroupInstanceId == groupInstance && x.StudentId == student).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<TestInstance>> GetTestInstanceToAssgin()
        {
            return await _testinstances
             .Where(x => x.CorrectionTeacherId == null && x.Status == (int)TestInstanceEnum.Solved).ToListAsync();
        }
        public virtual async Task<IReadOnlyList<TestInstance>> GetAllTestInstancesResults(int groupInstance)
        {
            return await _testinstances
                  .Include(x => x.Student)
                  .Include(x => x.LessonInstance)
                  .ThenInclude(x => x.GroupInstance)
                  .Where(x => x.LessonInstance.GroupInstanceId == groupInstance && x.Status == (int)TestInstanceEnum.Corrected).ToListAsync();
        }
        public virtual int GetAllTestInstancesResultsCount(int groupInstance)
        {
            return _testinstances
                .Include(x => x.Student)
                .Include(x => x.LessonInstance)
                .ThenInclude(x => x.GroupInstance)
                .Where(x => x.LessonInstance.GroupInstanceId == groupInstance && x.Status == (int)TestInstanceEnum.Corrected).Count();
        }
        public virtual async Task<IReadOnlyList<object>> GetAllClosedAndPendingQuizzAsync(int GroupInstanceId)
        {
            return await _testinstances
                  .Include(x => x.LessonInstance)
                  .Where(x => (x.Status == (int)TestInstanceEnum.Closed || x.Status == (int)TestInstanceEnum.Pending) && x.Test.TestTypeId == (int)TestTypeEnum.quizz).Where(x => x.LessonInstance.GroupInstanceId == GroupInstanceId).Select(x => x.LessonInstance).Distinct().ToListAsync();
        }


        public virtual async Task<List<TestInstance>> GetTestInstanceByLessonInstanceId(int LessonInstanceId)
        {
            return await _testinstances.Where(x => x.LessonInstanceId == LessonInstanceId).ToListAsync();
        }

        public async Task<bool> checkNotExiset(int quizId, int LessonId)
        {
            return !await _testinstances.Where(x => x.TestId == quizId && x.LessonInstanceId == LessonId).AnyAsync();
        }

    }
}
