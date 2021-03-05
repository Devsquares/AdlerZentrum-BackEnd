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
        private readonly ApplicationDbContext _context;

        public AdlerCardsUnitRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _adlercardsunits = dbContext.Set<AdlerCardsUnit>();
            _context = dbContext;
        }

        public List<GetAdlerCardUnitsForStudentViewModel> GetAdlerCardUnitsForStudent(string studentId, int levelId, int adlerCardTypeId)
        {
            var units = _adlercardsunits.Include(x => x.AdlerCards).Where(x => x.LevelId == levelId && x.AdlerCardsTypeId == adlerCardTypeId).ToList();
            return null;
        }

        public List<AdlerCardsUnit> GetAdlerCardUnitsByLevelAndType( int levelId, int adlerCardTypeId)
        {
            var units = _adlercardsunits.Include(x => x.Level).Where(x => x.LevelId == levelId && x.AdlerCardsTypeId == adlerCardTypeId).ToList();
            return units;
        }


    }

}
