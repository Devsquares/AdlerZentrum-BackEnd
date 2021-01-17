using Application.Enums;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class GroupDefinitionRepositoryAsync : GenericRepositoryAsync<GroupDefinition>, IGroupDefinitionRepositoryAsync
    {
        private readonly DbSet<GroupDefinition> _groupDefinitions;
        private readonly DbSet<GroupInstance> _groupInstans;
        public GroupDefinitionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _groupDefinitions = dbContext.Set<GroupDefinition>();
            _groupInstans = dbContext.Set<GroupInstance>();
        }

        public void AddStudentToGroupDefinition(int groupDefinitionId,string studentid,int promocodeId)
        {
            var groupDefinition = _groupDefinitions.Include(x=>x.GroupCondition).Where(x => x.Id == groupDefinitionId).FirstOrDefault();
            var groupInstans = _groupInstans.Where(x => x.GroupDefinitionId == groupDefinitionId && x.Status == (int)GroupInstanceStatusEnum.Pending).FirstOrDefault();

        }
    }
}
