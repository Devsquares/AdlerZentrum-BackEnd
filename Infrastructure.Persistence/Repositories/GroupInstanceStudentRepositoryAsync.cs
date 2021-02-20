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

        public int GetCountOfStudents(int groupId)
        {
            return groupInstanceStudents.Where(x => x.GroupInstanceId == groupId).Count();
        }
        
          public List<GroupInstanceModel> GetAllLastByStudentId(string studentId)
        {
            return groupInstanceStudents.Include(x => x.GroupInstance.GroupDefinition)
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new GroupInstanceModel() { 
                    GroupDefinitionId = x.GroupInstance.GroupDefinitionId,
                    GroupDefinitionStartDate = x.GroupInstance.GroupDefinition.StartDate,
                    GroupDefinitionEndDate = x.GroupInstance.GroupDefinition.EndDate,
                    GroupDefinitionFinalTestDate = x.GroupInstance.GroupDefinition.FinalTestDate,
                    Serial = x.GroupInstance.Serial,
                    Status = x.GroupInstance.Status,
                    CreatedDate = x.GroupInstance.CreatedDate,
                    IsCurrent = x.IsDefault,
                    GroupInstanceId = x.GroupInstance.Id

                })
                .ToList();
        }
    }
}
