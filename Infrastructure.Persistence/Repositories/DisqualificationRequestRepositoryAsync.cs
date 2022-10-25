using Application.Filters;
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
    public class DisqualificationRequestRepositoryAsync : GenericRepositoryAsync<DisqualificationRequest>, IDisqualificationRequestRepositoryAsync
    {
        private readonly DbSet<DisqualificationRequest> _disqualificationrequests;


        public DisqualificationRequestRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _disqualificationrequests = dbContext.Set<DisqualificationRequest>();
        }
        public override async Task<IReadOnlyList<DisqualificationRequest>> GetPagedReponseAsync(FilteredRequestParameter filteredRequestParameter)
        {
            bool noPaging = filteredRequestParameter.NoPaging;
            if (noPaging)
            {
                filteredRequestParameter.PageNumber = 1;
                filteredRequestParameter.PageSize = FilteredRequestParameter.MAX_ELEMENTS;
            }
            int pageNumber = filteredRequestParameter.PageNumber;
            int pageSize = filteredRequestParameter.PageSize;

            string sortBy = filteredRequestParameter.SortBy;
            if (sortBy == null)
            {
                sortBy = "ID";
            }
            string sortType = filteredRequestParameter.SortType;
            bool sortASC = true;

            if (sortType.ToUpper().Equals("DESC"))
            {
                sortASC = false;
            }
            return await _disqualificationrequests
            .Include(x => x.Student)
                    .Where(IsMatchedExpression(filteredRequestParameter))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IReadOnlyList<DisqualificationRequest>> GetAll(int pageNumber, int pageSize, int? status)
        {
            var disqualificationrequests = new List<DisqualificationRequest>();
            if (status == null)
            {
                disqualificationrequests = await _disqualificationrequests
                                .Where(x => (status == null ? true : x.DisqualificationRequestStatus == (int)status))
                                .Include(x => x.Student)
                                .OrderBy(x => x.DisqualificationRequestStatus)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            }
            else
            {
                disqualificationrequests = await _disqualificationrequests
                                .Where(x => (status == null ? true : x.DisqualificationRequestStatus == (int)status))
                                .Include(x => x.Student).Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            }


            return disqualificationrequests;
        }

        public int GetAllCount(int? status)
        {

            return _disqualificationrequests.Where(x => (status == null ? true : x.DisqualificationRequestStatus == (int)status)).Count();
        }
    }

}
