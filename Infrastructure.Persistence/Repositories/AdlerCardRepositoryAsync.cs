using Application.Enums;
using Application.Features;
using Application.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Models;
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
    public class AdlerCardRepositoryAsync : GenericRepositoryAsync<AdlerCard>, IAdlerCardRepositoryAsync
    {
        private readonly DbSet<AdlerCard> _adlercards;
        private readonly ApplicationDbContext _context;


        public AdlerCardRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _adlercards = dbContext.Set<AdlerCard>();
            _context = dbContext;
        }
        public List<GetAdlerCardGroupsForStudentViewModel> GetAdlerCardGroupsForStudent()
        {
            var res = _adlercards.Include(x => x.Level).ToList();
            var res1 = res.GroupBy(x => new { x.LevelId, x.AdlerCardsTypeId }).ToList();
            List<GetAdlerCardGroupsForStudentViewModel> lst = new List<GetAdlerCardGroupsForStudentViewModel>();
            foreach (var item in res1)
            {
                lst.Add(new GetAdlerCardGroupsForStudentViewModel
                {
                    AdlerCardTypeId = item.Key.AdlerCardsTypeId,
                    LevelId = item.Key.LevelId,
                    NoOfCards = item.Count(),
                    LevelName = item.Select(x=>x.Level.Name).FirstOrDefault()
                });
            }
            return lst;
        }

        public List<AdlerCard> GetAllByUnitId(int unitId)
        {
            var adlerCards = _adlercards.Where(x => x.AdlerCardsUnitId == unitId).ToList();
            return adlerCards;
        }

        public List<AdlerCardModel> GetAdlerCardsForStudent(string studentId, int adlerCardUnitId)
        {
            var query = (from ac in _adlercards
                         join acs in _context.AdlerCardSubmissions on ac.Id equals acs.AdlerCardId into gj
                         from x in gj.DefaultIfEmpty()
                         where ac.AdlerCardsUnitId == adlerCardUnitId || x.StudentId == studentId
                         select new AdlerCardModel() {
                             Name = ac.Name,
                             AdlerCardsUnitId = ac.AdlerCardsUnitId,
                             Question = ac.Question,
                             QuestionId = ac.QuestionId,
                             AllowedDuration = ac.AllowedDuration,
                             TotalScore = ac.TotalScore,
                             Status = (x == null) ? "unsolved" : "solved",
                             AdlerCardsTypeId = ac.AdlerCardsTypeId,
                             LevelId = ac.LevelId,
                             Level = ac.Level
                         }).ToList();
            return query;
        }
        public override async Task<IReadOnlyList<AdlerCard>> GetPagedReponseAsync(FilteredRequestParameter filteredRequestParameter)
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
            return await _adlercards
                .Include(x => x.AdlerCardsUnit)
                .Include(x => x.Level)
                .Include(x => x.Question)
                .Where(IsMatchedExpression(filteredRequestParameter))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    //.OrderBy(sortBy, sortASC)
                    .AsNoTracking()
                    .ToListAsync();

        }

    }

}
