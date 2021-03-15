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
    public class LevelRepositoryAsync : GenericRepositoryAsync<Level>, ILevelRepositoryAsync
    {
        private readonly DbSet<Level> _levels;

        public LevelRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _levels = dbContext.Set<Level>();
        }

        public async Task<Level> GetById(int Id)
        {
            return await _levels.Include(x => x.SubLevels).Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        //public override async Task<IReadOnlyList<Level>> GetPagedReponseAsync(FilteredRequestParameter filteredRequestParameter)
        //{
        //    bool noPaging = filteredRequestParameter.NoPaging;
        //    if (noPaging)
        //    {
        //        filteredRequestParameter.PageNumber = 1;
        //        filteredRequestParameter.PageSize = FilteredRequestParameter.MAX_ELEMENTS;
        //    }
        //    int pageNumber = filteredRequestParameter.PageNumber;
        //    int pageSize = filteredRequestParameter.PageSize;

        //    string sortBy = filteredRequestParameter.SortBy;
        //    if (sortBy == null)
        //    {
        //        sortBy = "ID";
        //    }
        //    string sortType = filteredRequestParameter.SortType;
        //    bool sortASC = true;

        //    if (sortType.ToUpper().Equals("DESC"))
        //    {
        //        sortASC = false;
        //    }
        //    return await _levels
        //        .Include(x => x.SubLevels)
        //        .Where(IsMatchedExpression(filteredRequestParameter))
        //            .Skip((pageNumber - 1) * pageSize)
        //            .Take(pageSize)
        //            //.OrderBy(sortBy, sortASC)
        //            .AsNoTracking()
        //            .ToListAsync();

        //}
    }
}
