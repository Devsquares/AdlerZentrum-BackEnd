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

        public TeacherGroupInstanceAssignment GetByTeacherId(string TeacherId)
        {
            return groupInstances.Include(x => x.GroupInstance).Where(x => x.TeacherId == TeacherId).FirstOrDefault();
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

        public List<TeacherGroupInstanceAssignment> GetAll(int pageNumber, int pageSize, out int totalCount, int? sublevelId = null, int? groupDefinationId = null)
        {
            var query = groupInstances.Include(x => x.GroupInstance)
             .Where(x => (sublevelId != null ? x.GroupInstance.GroupDefinition.SubLevelId == sublevelId.Value : true)
              && (groupDefinationId != null ? x.GroupInstance.GroupDefinitionId == groupDefinationId.Value : true))
              .AsQueryable();
            totalCount = query.Count();
            var list = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return list;
        }

        public TeacherGroupInstanceAssignment GetFirstNotIsDefault(int groupInstanceId)
        {
            return groupInstances.Include(x => x.Teacher).Where(x => x.GroupInstanceId == groupInstanceId && x.IsDefault == false).FirstOrDefault();
        }
        public TeacherGroupInstanceAssignment GetByGroupInstanceIdWithoutDefault(int groupInstanceId)
        {
            return groupInstances.Include(x => x.Teacher).Where(x => x.GroupInstanceId == groupInstanceId).FirstOrDefault();
        }
    }
}
