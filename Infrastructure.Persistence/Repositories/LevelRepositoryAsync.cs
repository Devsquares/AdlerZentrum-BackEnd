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
    public class LevelRepositoryAsync : GenericRepositoryAsync<Level>, ILevelRepositoryAsync
    {
        private readonly DbSet<Level> _levels;


        public LevelRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _levels = dbContext.Set<Level>();

        }
    }
}
