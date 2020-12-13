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
    public class GroupInstanceRepositoryAsync : GenericRepositoryAsync<GroupInstance>, IGroupInstanceRepositoryAsync
    {
        private readonly DbSet<GroupInstance> groupInstances;
        private readonly DbSet<GroupInstanceStudents> groupInstanceStudents;
        public GroupInstanceRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            groupInstances = dbContext.Set<GroupInstance>();
            groupInstanceStudents = dbContext.Set<GroupInstanceStudents>();
        }

        public int? GetActiveGroupInstance(string userId)
        {
            return groupInstanceStudents.Where(x => x.StudentId == userId && x.IsDefault == true).FirstOrDefault()?.GroupInstanceId;
        }

        public override async Task<IReadOnlyList<GroupInstance>> GetPagedReponseAsync(FilteredRequestParameter filteredRequestParameter)
        {
            bool noPaging = filteredRequestParameter.NoPaging;
            if (noPaging)
            {
                filteredRequestParameter.PageNumber = 1;
                filteredRequestParameter.PageSize = FilteredRequestParameter.MAX_ELEMENTS;
            }
            int pageNumber = filteredRequestParameter.PageNumber;
            int pageSize = filteredRequestParameter.PageSize;

            string sortBy = filteredRequestParameter.SortBy;
            if (sortBy == null)
            {
                sortBy = "ID";
            }
            string sortType = filteredRequestParameter.SortType;
            bool sortASC = true;

            if (sortType.ToUpper().Equals("DESC"))
            {
                sortASC = false;
            }
            return await groupInstances
                .Include(x => x.GroupDefinition)
                .Include(x => x.GroupDefinition.GroupCondition)
                .Include(x => x.GroupDefinition.TimeSlot)
                .Include(x => x.GroupDefinition.Sublevel)
                .Include(x => x.GroupDefinition.TimeSlot.TimeSlotDetails)
                .Where(IsMatchedExpression(filteredRequestParameter))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    //.OrderBy(sortBy, sortASC)
                    .AsNoTracking()
                    .ToListAsync();

        }

        public IReadOnlyList<GroupInstanceStudents> GetStudents(int groupId)
        {
            return groupInstances.Include(x => x.Students).Where(x => x.Id == groupId).FirstOrDefault()?.Students.ToList();
        }
    }
}
