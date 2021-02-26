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
    public class AdlerCardsUnitRepositoryAsync : GenericRepositoryAsync<AdlerCardsUnit>, IAdlerCardsUnitRepositoryAsync
    {
        private readonly DbSet<AdlerCardsUnit> _adlercardsunits;


        public AdlerCardsUnitRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _adlercardsunits = dbContext.Set<AdlerCardsUnit>();
        }

        public List<GetAdlerCardUnitsForStudentViewModel> GetAdlerCardUnitsForStudent(string studentId, int levelId, int adlerCardTypeId)
        {
            var units = _adlercardsunits.Include(x => x.AdlerCards).Where(x => x.LevelId == levelId && x.AdlerCardsTypeId == adlerCardTypeId).ToList();
            return null;
        }
    }

}
