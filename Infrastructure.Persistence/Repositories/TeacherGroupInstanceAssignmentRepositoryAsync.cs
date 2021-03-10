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
            return groupInstances.Include(x => x.GroupInstance).Where(x => x.TeacherId == TeacherId);
        }

        public TeacherGroupInstanceAssignment GetByGroupInstanceId(int groupInstanceId)
        {
            return groupInstances.Include(x => x.Teacher).Where(x => x.GroupInstanceId == groupInstanceId && x.IsDefault == true).FirstOrDefault();
        }
        public TeacherGroupInstanceAssignment GetByTeachGroupInstanceId(string TeacherId, int groupInstanceId)
        {
            return groupInstances.Where(x => x.GroupInstanceId == groupInstanceId && x.TeacherId == TeacherId).FirstOrDefault();
        }
        public List<TeacherGroupInstanceAssignment> GetListByGroupInstanceId(int groupInstanceId)
        {
            return groupInstances.Include(x => x.Teacher).Where(x => x.GroupInstanceId == groupInstanceId).ToList();
        }
    }
}
