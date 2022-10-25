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

        public async Task<IReadOnlyList<BanRequest>> GetAll(int pageNumber, int pageSize, int? status)
        {
            var banrequests = new List<BanRequest>();
            if (status == null)
            {
                banrequests = await _banrequests
                                .Where(x => (status == null ? true : x.BanRequestStatus == (int)status))
                                .Include(x => x.Student)
                                .OrderBy(x => x.BanRequestStatus)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            }
            else
            {
                banrequests = await _banrequests
                                .Where(x => (status == null ? true : x.BanRequestStatus == (int)status))
                                .Include(x => x.Student).Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            }


            return banrequests;
        }

        public int GetAllCount(int? status)
        {

            return _banrequests.Where(x => (status == null ? true : x.BanRequestStatus == (int)status)).Count();
        }
    }

}
