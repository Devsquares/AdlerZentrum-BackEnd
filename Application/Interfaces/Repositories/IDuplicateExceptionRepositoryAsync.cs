using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IDuplicateExceptionRepositoryAsync : IGenericRepositoryAsync<DuplicateException>
    {
         DuplicateException GetByEmail(string email);
          bool check(string email);
    }
}
