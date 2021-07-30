using Application.Enums;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Models;
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
        private readonly DbSet<PlacementRelease> _placementReleases;
        public TestRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            tests = dbContext.Set<Test>();
            _placementReleases = dbContext.Set<PlacementRelease>();
        }

        public async Task<IReadOnlyList<TestsViewModel>> GetPagedReponseAsync(int pageNumber, int pageSize, int? testtype = null, int? levelId = null, int? subLevelId = null, int? testStatus = null)
        {
            IQueryable<Test> test = tests
                .Include(x => x.LessonDefinition)
                .Include(x => x.Sublevel)
                .Include(x => x.Level);
            if (testtype != null)
            {
                test = test.Where(x => x.TestTypeId == testtype);
            }
            if (levelId != null)
            {
                test = test.Where(x => x.LevelId == levelId);
            }
            if (subLevelId != null)
            {
                test = test.Where(x => x.SublevelId == subLevelId);
            }
            if (testStatus != null)
            {
                test = test.Where(x => x.Status == testStatus);
            }
            return await test
                  .Select(x => new TestsViewModel()
                  {
                      Id = x.Id,
                      Name = x.Name,
                      TestTypeId = x.TestTypeId,
                      LessonDefinition = x.LessonDefinition,
                      LevelId = x.LevelId,
                      LevelName = x.Level != null ? x.Level.Name : string.Empty,
                      SublevelId = x.SublevelId,
                      SubLevelName = x.Sublevel != null ? x.Sublevel.Name : string.Empty,
                      Status = x.Status,
                      StatusName = (Enum.Parse<TestStatusEnum>(x.Status.ToString())).ToString()
                  })
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .AsNoTracking()
                  .ToListAsync();
        }

        public async Task<IReadOnlyList<TestsViewModel>> GetPlacementPagedReponseAsync(int pageNumber, int pageSize, int? testStatus = null)
        {
            IQueryable<PlacementRelease> placementReleases = _placementReleases
                .Include(x => x.Test)
                .ThenInclude(x => x.LessonDefinition)
                .ThenInclude(x => x.Sublevel)
                .ThenInclude(x => x.Level);

            if (testStatus != null)
            {
                placementReleases = placementReleases.Where(x => x.Test.Status == testStatus && x.RelaeseDate < DateTime.Now);
            }
            return await placementReleases.Where(x => x.Cancel == false)
                  .Select(x => new TestsViewModel()
                  {
                      Id = x.Test.Id,
                      Name = x.Test.Name,
                      TestTypeId = x.Test.TestTypeId,
                      LessonDefinition = x.Test.LessonDefinition,
                      Status = x.Test.Status,
                      Date = x.RelaeseDate,
                      Duration = x.Test.TestDuration,
                      StatusName = (Enum.Parse<TestStatusEnum>(x.Test.Status.ToString())).ToString()
                  })
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .AsNoTracking()
                  .ToListAsync();
        }

        public int GetPlacementCount()
        {
            return _placementReleases.Where(x => x.Cancel == false && x.RelaeseDate <= DateTime.Now).Count();
        }

        public int GetCount(int? testtype = null, int? levelId = null, int? subLevelId = null, int? testStatus = null)
        {
            IQueryable<Test> test = tests;

            if (testtype != null)
            {
                test = test.Where(x => x.TestTypeId == testtype);
            }
            if (levelId != null)
            {
                test = test.Where(x => x.LevelId == levelId);
            }
            if (subLevelId != null)
            {
                test = test.Where(x => x.SublevelId == subLevelId);
            }
            if (testStatus != null)
            {
                test = test.Where(x => x.Status == testStatus);
            }
            return test.Count();
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
            return await tests.Where(x => x.SublevelId == Sublevel && x.TestTypeId == (int)TestTypeEnum.subLevel).OrderBy(r => Guid.NewGuid()).Take(5).FirstOrDefaultAsync();
        }

        public async Task<Test> GetFeedbackSheet()
        {
            return await tests.Include(x => x.Questions).ThenInclude(x => x.SingleQuestions).ThenInclude(x=>x.Choices).Where(x => x.IsArchived == false && x.TestTypeId == (int)TestTypeEnum.Feedback).FirstOrDefaultAsync();
        }

        public async Task<Test> GetFinalLevelTestBySublevelAsync(int level)
        {
            return await tests
            .Where(x => x.LevelId == level && x.TestTypeId == (int)TestTypeEnum.final).OrderBy(r => Guid.NewGuid()).Take(5).FirstOrDefaultAsync();
        }

        public async Task<int> FeedbackSheetNotArchivedCount()
        {
            return await tests.Where(x => x.IsArchived == false && x.TestTypeId == (int)TestTypeEnum.Feedback).CountAsync();
        }
    }
}
