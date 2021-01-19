using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class GroupDefinitionRepositoryAsync : GenericRepositoryAsync<GroupDefinition>, IGroupDefinitionRepositoryAsync
    {
        private readonly DbSet<GroupDefinition> groupDefinitions;
        public GroupDefinitionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            groupDefinitions = dbContext.Set<GroupDefinition>();
        }
    }
}
