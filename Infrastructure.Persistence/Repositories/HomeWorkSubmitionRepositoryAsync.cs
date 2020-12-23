using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class HomeWorkSubmitionRepositoryAsync : GenericRepositoryAsync<HomeWorkSubmition>, IHomeWorkSubmitionRepositoryAsync
    {
        private readonly DbSet<HomeWorkSubmition> homeWorkSubmitions;
        private readonly DbSet<LessonInstance> lessonInstances;
        public HomeWorkSubmitionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            homeWorkSubmitions = dbContext.Set<HomeWorkSubmition>();
        }

        public async Task<IReadOnlyList<HomeWorkSubmition>> GetAllAsync(int groupInstanceId)
        {
            return await homeWorkSubmitions
                .Include(x => x.Homework)
            .Where(x => x.Homework.GroupInstanceId == groupInstanceId).ToListAsync();
        }

        public async Task<IReadOnlyList<HomeWorkSubmition>> GetAllForStudentAsync(string studentId, int groupInstanceId)
        {
            return await homeWorkSubmitions
                .Include(x => x.Homework)
                .Include(x => x.Homework.LessonInstance)
             .Where(x => x.StudentId == studentId && x.Homework.GroupInstanceId == groupInstanceId).ToListAsync();
        }
        public async Task<HomeWorkSubmition> GetByIdAsync(int id)
        {
            return await homeWorkSubmitions.Include(x => x.Homework).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

    }
}
