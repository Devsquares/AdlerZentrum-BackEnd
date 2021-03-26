using Application.Enums;
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
        public HomeWorkSubmitionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            homeWorkSubmitions = dbContext.Set<HomeWorkSubmition>();
        }

        public async Task<IReadOnlyList<HomeWorkSubmition>> GetAllByGroupInstanceAsync(int groupInstanceId, int? Status)
        {
            return await homeWorkSubmitions
                .Include(x => x.Homework)
                .Include(x => x.Student)
                .Include(x => x.Homework.GroupInstance)
                .Include(x => x.Homework.LessonInstance)
            .Where(x => x.Homework.GroupInstanceId == groupInstanceId && Status != null ? x.Status == Status : true).ToListAsync();
        }

        public async Task<IReadOnlyList<HomeWorkSubmition>> GetAllByTeacherIdAsync(string TeacherId, int? Status)
        {
            return await homeWorkSubmitions
                .Include(x => x.Homework)
                .Include(x => x.Student)
                .Include(x => x.Homework.GroupInstance)
                .Include(x => x.Homework.LessonInstance)
                .Where(x => x.Homework.TeacherId == TeacherId && Status != null ? x.Status == Status : true)
                .ToListAsync();
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
            return await homeWorkSubmitions.Include(x => x.Homework.LessonInstance).Include(x => x.Student).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

    }
}
