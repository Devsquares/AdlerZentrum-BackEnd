using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IQuestionRepositoryAsync : IGenericRepositoryAsync<Question>
    {
        Task<IReadOnlyList<Question>> GetAllByTypeIdAsync(int questionTypeId, int pageNumber, int pageSize);
        int GetAllByTypeIdCountAsync(int questionTypeId);
        Task<List<Question>> GetByTestIdAsync(int id);
        Task<IReadOnlyList<Question>> GetAllAsync(int pageNumber, int pageSize, int? questionTypeId = null);
    }
}
