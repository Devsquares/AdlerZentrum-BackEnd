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
    }

}
