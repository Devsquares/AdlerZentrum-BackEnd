using Application.Features;
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
    public class AdlerCardRepositoryAsync : GenericRepositoryAsync<AdlerCard>, IAdlerCardRepositoryAsync
    {
        private readonly DbSet<AdlerCard> _adlercards;


        public AdlerCardRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _adlercards = dbContext.Set<AdlerCard>();
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
    }

}
