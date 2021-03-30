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
            var query = (from acu in _adlercardsunits
                        join acs in _context.AdlerCardSubmissions on acu.Id equals acs.AdlerCard.AdlerCardsUnitId into gj
                        from x in gj.DefaultIfEmpty()
                        where acu.LevelId == levelId && acu.AdlerCardsTypeId == adlerCardTypeId &&  (!string.IsNullOrEmpty(studentId)?x.StudentId==studentId:true)
                        select new GetAdlerCardUnitsForStudentViewModel()
                        {
                            AdlerCardUnitId = acu.Id,
                            AdlerCardUnitName = acu.Name,
                            AdlerCardUnitImage = acu.Image,
                            AdlerCardUnitDescription = acu.Description,
                            AdlerCardUnitCount = acu.AdlerCards.Where(x=>x.Status == (int)AdlerCardEnum.Open).Count(),
                            AdlerCardUnitAchievedCount =x != null? _context.AdlerCardSubmissions.Where(x=>x.StudentId == studentId).Count():0,
                            Levels = x.AdlerCard.Level
                        }).ToList();

            //var units = _adlercardsunits.Include(x => x.AdlerCards).Where(x => x.LevelId == levelId && x.AdlerCardsTypeId == adlerCardTypeId)
            //    .Select(x=>new GetAdlerCardUnitsForStudentViewModel() { 
            //        AdlerCardUnitId = x.Id,
            //        AdlerCardUnitName = x.Name,
            //        AdlerCardUnitImage = x.Image,
            //        AdlerCardUnitDescription = x.Description
            //    })
            //    .ToList();
            return query;
        }

        public List<AdlerCardsUnit> GetAdlerCardUnitsByLevelAndType( int levelId, int adlerCardTypeId)
        {
            var units = _adlercardsunits.Include(x => x.Level).Where(x => x.LevelId == levelId && x.AdlerCardsTypeId == adlerCardTypeId).ToList();
            return units;
        }

        public AdlerCardsUnit GetAdlerCardsUnitById(int id)
        {
            return _adlercardsunits.Include(x=>x.Level).Where(x => x.Id == id).FirstOrDefault();
        }

    }

}
