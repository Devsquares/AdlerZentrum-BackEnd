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
        public GroupInstanceStudentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            groupInstanceStudents = dbContext.Set<GroupInstanceStudents>();
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
    }
}
