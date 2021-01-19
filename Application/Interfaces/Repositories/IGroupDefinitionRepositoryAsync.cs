using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface IGroupDefinitionRepositoryAsync : IGenericRepositoryAsync<GroupDefinition>
    {
        List<GroupDefinition> GetAvailableGroupDefinitionsForStudent(int sublevelId);
    }
}
