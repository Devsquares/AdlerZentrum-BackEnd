using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IPromoCodeInstanceRepositoryAsync : IGenericRepositoryAsync<PromoCodeInstance>
    {
        List<PromoCodeInstancesViewModel> GetAllReport(int pageNumber, int pageSize, out int count, int? promocodeId = null, bool? isValid = null, string promoCodeName = null, string studentName = null);
        PromoCodeInstancesViewModel GetByPromoCodeKey(string promoKey);
    }
}
