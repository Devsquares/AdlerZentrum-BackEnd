using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IPromoCodeRepositoryAsync : IGenericRepositoryAsync<PromoCode>
    {
        PromoCode CheckPromoCode(string name);
        PromoCode GetByName(string name);
    }
}
