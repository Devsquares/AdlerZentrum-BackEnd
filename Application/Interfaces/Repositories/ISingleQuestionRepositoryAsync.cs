using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ISingleQuestionRepositoryAsync : IGenericRepositoryAsync<SingleQuestion>
    {
        Task<IReadOnlyList<SingleQuestion>> GetPagedReponseAsync(int pageNumber, int pageSize, int typeId); Task<IReadOnlyList<SingleQuestion>> GetAllByIdAsync(List<int> Ids);
    }
}
