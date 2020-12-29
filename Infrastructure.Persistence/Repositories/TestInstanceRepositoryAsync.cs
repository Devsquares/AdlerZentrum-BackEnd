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

    }
}
