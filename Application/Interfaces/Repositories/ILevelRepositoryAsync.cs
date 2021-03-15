using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ILevelRepositoryAsync : IGenericRepositoryAsync<Level>
    {
        Task<Level> GetById(int Id);
    }
}
