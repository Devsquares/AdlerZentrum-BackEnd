using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class GroupConditionRepositoryAsync : GenericRepositoryAsync<GroupCondition>, IGroupConditionRepositoryAsync
    {
        private readonly DbSet<GroupCondition> groupConditions;
        public GroupConditionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            groupConditions = dbContext.Set<GroupCondition>();
        }
    }
}
