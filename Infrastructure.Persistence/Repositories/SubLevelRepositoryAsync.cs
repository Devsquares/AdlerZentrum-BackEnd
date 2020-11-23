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
    public class SubLevelRepositoryAsync : GenericRepositoryAsync<Sublevel>, ISublevelRepositoryAsync
    {
        private readonly DbSet<Sublevel> _subLevels;


        public SubLevelRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _subLevels = dbContext.Set<Sublevel>();
        }
    }
}
