using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class TeacherGroupInstanceAssignmentRepositoryAsync : GenericRepositoryAsync<TeacherGroupInstanceAssignment>, ITeacherGroupInstanceAssignmentRepositoryAsync
    {
        private readonly DbSet<TeacherGroupInstanceAssignment> groupInstances;
        public TeacherGroupInstanceAssignmentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            groupInstances = dbContext.Set<TeacherGroupInstanceAssignment>();
        }

        public IEnumerable<TeacherGroupInstanceAssignment> GetByTeacher(string TeacherId)
        {
            return groupInstances.Where(x => x.TeacherId == TeacherId);
        }
    }
}
