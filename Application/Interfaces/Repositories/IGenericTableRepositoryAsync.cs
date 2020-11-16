using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Filters;

namespace Application.Interfaces.Repositories
{
    public interface IGenericTableRepositoryAsync
    {
        Task<int> GetCount(string table);
    }
}
