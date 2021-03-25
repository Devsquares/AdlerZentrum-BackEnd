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
    public class LessonInstanceRepositoryAsync : GenericRepositoryAsync<LessonInstance>, ILessonInstanceRepositoryAsync
    {
        private readonly DbSet<LessonInstance> lessonInstances;
        public LessonInstanceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            lessonInstances = dbContext.Set<LessonInstance>();
        }
        public IEnumerable<LessonInstance> GetByGroupInstanceId(int GroupInstanceId)
        {
            return lessonInstances
                .Include(x => x.GroupInstance)
                .Where(x => x.GroupInstanceId == GroupInstanceId).ToList();
        }
        public async override Task<LessonInstance> GetByIdAsync(int id)
        {
            return await lessonInstances.Include(x => x.LessonDefinition).ThenInclude(x => x.Sublevel).Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
