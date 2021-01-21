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
    public class TestRepositoryAsync : GenericRepositoryAsync<Test>, ITestRepositoryAsync
    {
        private readonly DbSet<Test> tests;
        public TestRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            tests = dbContext.Set<Test>();
        }

        public async Task<IReadOnlyList<Test>> GetPagedReponseAsync(int pageNumber, int pageSize, int type)
        {
            return await tests
                .Include(x => x.LessonDefinition)
                .ThenInclude(x => x.Sublevel)
                .Where(x => x.TestTypeId == type)
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .AsNoTracking()
                  .ToListAsync();
        }

        public virtual async Task<Test> GetByIdAsync(int id)
        {
            return await tests
                .Include(x => x.Questions)
                .ThenInclude(x => x.SingleQuestions)
                .ThenInclude(x => x.Choices)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Test> GetQuizzByLessonDefinationAsync(int lessonDefinationdId)
        {
            return await tests.Where(x => x.LessonDefinitionId == lessonDefinationdId && x.TestTypeId == (int)TestTypeEnum.quizz).FirstOrDefaultAsync();
        }

        public async Task<Test> GetSubLevelTestBySublevelAsync(int Sublevel)
        {
            return await tests
            .Include(x => x.LessonDefinition)
            .Where(x => x.LessonDefinition.SublevelId == Sublevel && x.TestTypeId == (int)TestTypeEnum.subLevel).FirstOrDefaultAsync();
        }

        public async Task<Test> GetFinalLevelTestBySublevelAsync(int level)
        {
            return await tests
            .Include(x => x.LessonDefinition)
            .ThenInclude(x => x.Sublevel)
            .Where(x => x.LessonDefinition.Sublevel.LevelId == level && x.TestTypeId == (int)TestTypeEnum.final).FirstOrDefaultAsync();
        }
    }
}
