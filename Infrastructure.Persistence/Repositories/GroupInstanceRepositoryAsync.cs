using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class GroupInstanceRepositoryAsync : GenericRepositoryAsync<GroupInstance>, IGroupInstanceRepositoryAsync
    {
        private readonly DbSet<GroupInstance> groupInstances;
        public GroupInstanceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            groupInstances = dbContext.Set<GroupInstance>();
        }
    }
}
