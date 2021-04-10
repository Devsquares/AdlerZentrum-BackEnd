using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class TeacherAbsenceRepositoryAsync : GenericRepositoryAsync<TeacherAbsence>, ITeacherAbsenceRepositoryAsync
    {
        private readonly DbSet<TeacherAbsence> _teacherabsences;


        public TeacherAbsenceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _teacherabsences = dbContext.Set<TeacherAbsence>();

        }
    }

}
