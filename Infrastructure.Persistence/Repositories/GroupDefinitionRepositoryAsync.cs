using Application.Enums;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class GroupDefinitionRepositoryAsync : GenericRepositoryAsync<GroupDefinition>, IGroupDefinitionRepositoryAsync
    {
        private readonly DbSet<GroupDefinition> groupDefinitions;
        public GroupDefinitionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            groupDefinitions = dbContext.Set<GroupDefinition>();
        }

        public  List<GroupDefinition> GetAvailableGroupDefinitionsForStudent(int sublevelId)
        {
            return groupDefinitions.Include(x=>x.Sublevel)
                .Include(x => x.GroupCondition)
                .Include(x => x.TimeSlot)
                .Where(x => x.Status == (int)GroupDefinationStatusEnum.Pending && x.SubLevelId == sublevelId).ToList();
        }
    }
}
