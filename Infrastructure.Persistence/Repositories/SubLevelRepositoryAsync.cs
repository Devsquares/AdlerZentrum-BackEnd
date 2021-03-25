using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Sublevel GetNextById(int id)
        {
            var current = _subLevels.Where(x => x.Id == id).FirstOrDefault();
            return GetNextByOrder(current.Order);
        }

        public Sublevel GetNextByOrder(int order)
        {
            return _subLevels.Where(x => x.Order == order + 1).FirstOrDefault();
        }

        public List<Sublevel> GetNotFinalSublevels()
        {
            return _subLevels.Include(x => x.Level).Where(x => x.IsFinal == false).ToList();
        }

        public Sublevel GetPreviousByOrder(int order)
        {
            return _subLevels.Where(x => x.Order == order - 1).FirstOrDefault();
        }
    }
}
