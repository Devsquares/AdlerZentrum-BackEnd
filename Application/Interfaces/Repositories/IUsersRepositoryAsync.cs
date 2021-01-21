using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUsersRepositoryAsync : IGenericRepositoryAsync<ApplicationUser>
    {
        ApplicationUser GetUserById(string Id);
    }
}
