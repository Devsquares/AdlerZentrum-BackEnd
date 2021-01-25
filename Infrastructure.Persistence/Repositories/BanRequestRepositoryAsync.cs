using Application.Enums;
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
    public class BanRequestRepositoryAsync : GenericRepositoryAsync<BanRequest>, IBanRequestRepositoryAsync
    {
        private readonly DbSet<BanRequest> _banrequests;


        public BanRequestRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _banrequests = dbContext.Set<BanRequest>();

        }

        public async Task<IReadOnlyList<BanRequest>> GetAllNew(int pageNumber, int pageSize)
        {
            return await _banrequests
                .Include(x => x.Student).Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .Where(x => x.BanRequestStatus == (int)BanRequestStatusEnum.New)
                .ToListAsync();
        }

        public int GetAllNewCount()
        {
            return _banrequests.Where(x => x.BanRequestStatus == (int)BanRequestStatusEnum.New).Count();
        }
    }

}
