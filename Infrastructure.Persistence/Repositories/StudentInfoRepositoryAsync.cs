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
    public class StudentInfoRepositoryAsync : GenericRepositoryAsync<StudentInfo>, IStudentInfoRepositoryAsync
    {
        private readonly DbSet<StudentInfo> _studentInfo;


        public StudentInfoRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _studentInfo = dbContext.Set<StudentInfo>();
        }

        public StudentInfo GetUserById(string Id)
        {
            return _studentInfo.Where(x => x.StudentId == Id).FirstOrDefault();
        }
        public bool CheckBySublevel(string studentId, int SublevelId)
        {
            return _studentInfo.Where(x => x.StudentId == studentId && x.SublevelId == SublevelId && x.SublevelSuccess == true).Any();
        }
    }
}
