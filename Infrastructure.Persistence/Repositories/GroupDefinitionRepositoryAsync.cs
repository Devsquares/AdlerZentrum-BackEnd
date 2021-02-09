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

        public List<GroupDefinition> GetALL(int pageNumber, int pageSize, string subLevelName, out int totalCount)
        {
            var groupDefinitionsList = groupDefinitions.Include(x => x.GroupCondition).Include(x => x.Sublevel).Where(x => (!string.IsNullOrEmpty(subLevelName) ? (x.Sublevel.Name.ToLower() == subLevelName.ToLower()) : true)).ToList();
            totalCount = groupDefinitionsList.Count();
            groupDefinitionsList = groupDefinitionsList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return groupDefinitionsList;
        }

        public GroupDefinition GetById(int groupDefinitionId)
        {
            return groupDefinitions
            .Include(x => x.TimeSlot)
            .Include(x => x.GroupCondition)
            .Include(x => x.Sublevel)
            .Include(x => x.Pricing)
            .Where(x => x.Id == groupDefinitionId).FirstOrDefault();
        }
    }
}
