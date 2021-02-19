using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IBugRepositoryAsync : IGenericRepositoryAsync<Bug>
    {

        Task<IReadOnlyList<Bug>> GetPagedReponseAsync(int pageNumber, int pageSize, string type, string status, string priority);
        int GetCount(string type, string status, string priority);
    }
}
