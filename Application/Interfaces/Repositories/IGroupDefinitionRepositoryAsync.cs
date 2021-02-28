using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGroupDefinitionRepositoryAsync : IGenericRepositoryAsync<GroupDefinition>
    {
        List<GroupDefinition> GetALL(int pageNumber, int pageSize, string subLevelName, out int totalCount, int? sublevelId = null);
        GroupDefinition GetById(int groupDefinitionId);
    }
}
