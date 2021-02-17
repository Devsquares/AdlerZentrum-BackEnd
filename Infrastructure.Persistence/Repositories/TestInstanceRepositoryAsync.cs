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
        private readonly DbSet<SingleQuestionSubmission> _singleQuestionSubmissions;
        private readonly DbSet<Test> _tests;
        private readonly DbSet<Question> _questions;
        private readonly DbSet<SingleQuestion> _singleQuestions;
        private readonly DbSet<Choice> _choice;
        private readonly DbSet<ChoiceSubmission> _choiceSubmission;

        public TestInstanceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _testInstances = dbContext.Set<TestInstance>();
            _singleQuestionSubmissions = dbContext.Set<SingleQuestionSubmission>();
            _singleQuestions = dbContext.Set<SingleQuestion>();
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

        public async Task<IReadOnlyList<TestInstance>> GetAllTestsToManage()
        {
            return await _testInstances
            .Include(x => x.Test)
            .Include(x => x.Student)
            .Include(x => x.LessonInstance)
            .ThenInclude(x => x.GroupInstance).ToListAsync();
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

        public async Task<IReadOnlyList<TestInstance>> GetTestInstanceByTeacher(string correctionTeacherId, int status)
        {
            return await _testInstances
              .Include(x => x.Test)
              .Include(x => x.Student)
              .Include(x => x.LessonInstance)
              .ThenInclude(x => x.GroupInstance)
               .Where(x => x.CorrectionTeacherId == correctionTeacherId && x.Status == status).ToListAsync();
        }
    }
}
