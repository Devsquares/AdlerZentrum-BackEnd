using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IDisqualificationRequestRepositoryAsync : IGenericRepositoryAsync<DisqualificationRequest>
    {
        Task<IReadOnlyList<DisqualificationRequest>> GetAll(int pageNumber, int pageSize, int? status);
        int GetAllCount(int? status);
    }
}
