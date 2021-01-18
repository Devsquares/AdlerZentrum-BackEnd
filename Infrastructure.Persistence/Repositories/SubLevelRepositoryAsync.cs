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
    public class SublevelRepositoryAsync : GenericRepositoryAsync<Sublevel>, ISublevelRepositoryAsync
    {
        private readonly DbSet<Sublevel> _sublevels;


        public SublevelRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _sublevels = dbContext.Set<Sublevel>();

        }
    }

}
