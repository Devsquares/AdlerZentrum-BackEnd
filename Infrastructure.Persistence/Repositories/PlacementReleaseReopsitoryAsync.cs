using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class PlacementReleaseReopsitoryAsync : GenericRepositoryAsync<PlacementRelease>, IPlacementReleaseReopsitoryAsync
    {
        private readonly DbSet<PlacementRelease> PlacementRelease;

        public PlacementReleaseReopsitoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            PlacementRelease = dbContext.Set<PlacementRelease>();
        }

        public async Task<List<PlacementRelease>> GetByTest(int TestId)
        {
            return await PlacementRelease.Where(x => x.TestId == TestId).ToListAsync();
        }
    }
}
