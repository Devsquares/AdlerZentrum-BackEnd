using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IPromoCodeRepositoryAsync : IGenericRepositoryAsync<PromoCode>
    {
        bool CheckPromoCode(string name);
    }
}
