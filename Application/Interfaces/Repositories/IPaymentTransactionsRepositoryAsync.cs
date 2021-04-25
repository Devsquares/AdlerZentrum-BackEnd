using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IPaymentTransactionsRepositoryAsync : IGenericRepositoryAsync<PaymentTransaction>
    {
        IReadOnlyList<PaymentTransaction> GetFinancialAnalysisReport(DateTime? From, DateTime? To, string StudentName, int? GroupInstanceId, int? Category, int PageNumber, int PageSize, out int count);
    }
}
