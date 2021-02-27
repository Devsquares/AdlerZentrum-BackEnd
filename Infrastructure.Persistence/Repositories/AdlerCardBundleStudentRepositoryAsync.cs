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
    public class AdlerCardBundleStudentRepositoryAsync : GenericRepositoryAsync<AdlerCardBundleStudent>, IAdlerCardBundleStudentRepositoryAsync
    {
        private readonly DbSet<AdlerCardBundleStudent> _adlercardbundlestudents;


        public AdlerCardBundleStudentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _adlercardbundlestudents = dbContext.Set<AdlerCardBundleStudent>();

        }
        public AdlerCardBundleStudent GetByBundleID(int bundleId)
        {
            return _adlercardbundlestudents.Include(x=>x.Student).Include(x => x.AdlerCardsBundleId).Where(x => x.Id == bundleId).FirstOrDefault();
        }
        public override async Task<IReadOnlyList<AdlerCardBundleStudent>> GetPagedReponseAsync(FilteredRequestParameter filteredRequestParameter)
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
            return await _adlercardbundlestudents
                .Include(x => x.Student)
                .Include(x => x.AdlerCardsBundle)
                .Where(IsMatchedExpression(filteredRequestParameter))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    //.OrderBy(sortBy, sortASC)
                    .AsNoTracking()
                    .ToListAsync();

        }

    }

}
