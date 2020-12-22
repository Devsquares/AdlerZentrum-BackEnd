using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class LessonDefinitionRepositoryAsync : GenericRepositoryAsync<LessonDefinition>, ILessonDefinitionRepositoryAsync
    {
        private readonly DbSet<LessonDefinition> lessonDefinitions;
        public LessonDefinitionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            lessonDefinitions = dbContext.Set<LessonDefinition>();
        }

        public async Task<ICollection<LessonDefinition>> GetBySubLevelId(int SubLevelId)
        {
            return lessonDefinitions.Where(x => x.SublevelId == SubLevelId).ToList();
        }
    }
}
