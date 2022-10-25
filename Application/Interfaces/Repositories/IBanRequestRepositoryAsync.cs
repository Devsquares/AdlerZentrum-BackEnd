using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IBanRequestRepositoryAsync : IGenericRepositoryAsync<BanRequest>
    {
        Task<IReadOnlyList<BanRequest>> GetAllNew(int pageNumber, int pageSize);
        int GetAllNewCount();
        Task<IReadOnlyList<BanRequest>> GetAll(int pageNumber, int pageSize, int? status);
        int GetAllCount(int? status);
    }
}
