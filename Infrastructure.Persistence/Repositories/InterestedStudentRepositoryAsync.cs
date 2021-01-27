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
    public class InterestedStudentRepositoryAsync : GenericRepositoryAsync<InterestedStudent>, IInterestedStudentRepositoryAsync
    {
        private readonly DbSet<InterestedStudent> _interestedstudents;


        public InterestedStudentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _interestedstudents = dbContext.Set<InterestedStudent>();

        }

        public InterestedStudent GetByStudentId(string studentId, int groupDefinitionId)
        {
            return _interestedstudents.Where(x => x.StudentId == studentId && x.GroupDefinitionId == groupDefinitionId).FirstOrDefault();
        }

        public object GetByGroupDefinitionId( int groupDefinitionId)
        {
            var interestedStudents =  _interestedstudents.Include(x=>x.Student)
                .Include(x => x.GroupDefinition)
                .Include(x => x.PromoCode)
                .Where(x =>  x.GroupDefinitionId == groupDefinitionId)
                .Select(x=>new { 
                    StudentId = x.Student.Id,
                    StudentName =$"{x.Student.FirstName} {x.Student.LastName}",
                    PromocodeId = x.PromoCode.Id,
                    Promocode = x.PromoCode.Name
                }).ToList();
            return interestedStudents;
        }
    }

}
