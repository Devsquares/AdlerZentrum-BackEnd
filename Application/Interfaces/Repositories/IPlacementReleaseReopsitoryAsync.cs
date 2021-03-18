using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IPlacementReleaseReopsitoryAsync : IGenericRepositoryAsync<PlacementRelease>
    {
        Task<List<PlacementRelease>>  GetByTest(int TestId);
    }
}
