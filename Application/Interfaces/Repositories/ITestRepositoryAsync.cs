using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITestRepositoryAsync : IGenericRepositoryAsync<Test>
    {
        Task<IReadOnlyList<Test>> GetPagedReponseAsync(int pageNumber, int pageSize, int type);
    }
}
