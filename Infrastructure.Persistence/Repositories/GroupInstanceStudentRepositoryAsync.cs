using Application.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class GroupInstanceStudentRepositoryAsync : GenericRepositoryAsync<GroupInstanceStudents>, IGroupInstanceStudentRepositoryAsync
    {
        private readonly DbSet<GroupInstanceStudents> groupInstanceStudents;
        private readonly DbSet<GroupInstance> groupInstances;
        public GroupInstanceStudentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            groupInstanceStudents = dbContext.Set<GroupInstanceStudents>();
            groupInstances = dbContext.Set<GroupInstance>();
        }

        public GroupInstanceStudents GetByStudentId(string studentId,int groupId)
        {
            return groupInstanceStudents.Where(x => x.StudentId == studentId && x.GroupInstanceId == groupId).FirstOrDefault();
        }

        public int GetCountOfStudents(int groupId)
        {
            return groupInstanceStudents.Where(x => x.GroupInstanceId == groupId).Count();
        }

        public List<string> GetEmailsByGroupDefinationId(int groupDefinationId)
        {
           var emailList =   groupInstanceStudents.Include(x=>x.GroupInstance)
                .Include(x => x.Student)
                .Where(x => x.GroupInstance.GroupDefinitionId == groupDefinationId).Select(x=>x.Student.Email).ToList();
            return emailList;
        }

        public int GetCountOfPlacmentTestStudents(int groupId)
        {
            return groupInstanceStudents.Where(x => x.GroupInstanceId == groupId && x.IsPlacementTest == true).Count();
        }

        public int GetCountOfStudentsByGroupDefinitionId(int groupDefinitionId)
        {
            return groupInstanceStudents.Include(x=>x.GroupInstance).Where(x => x.GroupInstance.GroupDefinitionId == groupDefinitionId).Count();
        }

        public GroupInstance GetLastByStudentId(string studentId)
        {
            return groupInstanceStudents.Include(x=>x.GroupInstance.GroupDefinition).Where(x => x.StudentId == studentId ).OrderByDescending(x=>x.CreatedDate).Select(x=>x.GroupInstance).FirstOrDefault();
        }
        public List<GroupInstance> GetAllLastByStudentId(string studentId)
        {
            return groupInstanceStudents.Include(x => x.GroupInstance.GroupDefinition).Where(x => x.StudentId == studentId).OrderByDescending(x => x.CreatedDate).Select(x => x.GroupInstance).ToList();
        }

        public List<ApplicationUser> GetAllStudentInGroupInstanceByStudentId(string studentId)
        {
            var groupinstance = groupInstanceStudents.Where(x => x.StudentId == studentId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            return groupInstanceStudents.Include(x => x.Student).Where(x => x.GroupInstanceId == groupinstance.GroupInstanceId).Select(x => x.Student).ToList();

        }
        public List<ApplicationUser> GetAllStudentInGroupDefinitionByStudentId(string studentId)
        {
            var groupinstance = groupInstanceStudents.Include(x=>x.GroupInstance).Where(x => x.StudentId == studentId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            return groupInstanceStudents.Include(x => x.Student).Where(x => x.GroupInstance.GroupDefinitionId == groupinstance.GroupInstance.GroupDefinitionId).Select(x => x.Student).ToList();
        }
    }
}
