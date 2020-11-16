using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Filters;

namespace Application.Interfaces
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task<IReadOnlyList<T>> GetPagedReponseAsync(FilteredRequestParameter filteredRequestParameter);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        int GetCount(T entity);
        int GetCount(FilteredRequestParameter filteredRequestParameter);
    }
}
