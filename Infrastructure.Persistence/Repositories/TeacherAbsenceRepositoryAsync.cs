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
    public class TeacherAbsenceRepositoryAsync : GenericRepositoryAsync<TeacherAbsence>, ITeacherAbsenceRepositoryAsync
    {
        private readonly DbSet<TeacherAbsence> _teacherabsences;


        public TeacherAbsenceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _teacherabsences = dbContext.Set<TeacherAbsence>();

        }
        public override async Task<IReadOnlyList<TeacherAbsence>> GetPagedReponseAsync(FilteredRequestParameter filteredRequestParameter)
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
            return await _teacherabsences
                .Include(x => x.Teacher)
                .Include(x => x.LessonInstance)
                .Where(IsMatchedExpression(filteredRequestParameter))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    //.OrderBy(sortBy, sortASC)
                    .AsNoTracking()
                    .ToListAsync();

        }

        public async Task<List<TeacherAbsence>> GetAll(int pageNumber,int pageSize,int?status)
        {
            return await _teacherabsences
               .Include(x => x.Teacher)
               .Include(x => x.LessonInstance)
               .Where(x=> status.HasValue? x.Status == status.Value:true)
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   //.OrderBy(sortBy, sortASC)
                  // .AsNoTracking()
                   .ToListAsync();
        }

        public TeacherAbsence GetbyId(int id)
        {
            return _teacherabsences.Include(x => x.Teacher).Include(x => x.LessonInstance).Where(x => x.Id == id).FirstOrDefault
                ();
        }

    }

}
