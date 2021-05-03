using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    class PaymentTransactionsRepositoryAsync : GenericRepositoryAsync<PaymentTransaction>, IPaymentTransactionsRepositoryAsync
    {
        private readonly DbSet<PaymentTransaction> paymentTransactions;

        public PaymentTransactionsRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            paymentTransactions = dbContext.Set<PaymentTransaction>();
        }

        public IReadOnlyList<PaymentTransaction> GetFinancialAnalysisReport(DateTime? From, DateTime? To, string StudentName, int? GroupDefinitionId, int? Category, int PageNumber, int PageSize, out int count)
        {
            var query = paymentTransactions.AsQueryable();

            if (From.HasValue)
            {
                query = query.Where(x => x.PurchasingDate >= From);
            }

            if (To.HasValue)
            {
                query = query.Where(x => x.PurchasingDate <= To);
            }

            if (!string.IsNullOrWhiteSpace(StudentName))
            {
                var predicate = PredicateBuilder.New<PaymentTransaction>();
                string[] searchWordsArr = StudentName.Split(" ");
                foreach (var item in searchWordsArr)
                {
                    predicate.Or(x => x.User.FirstName.ToLower().Contains(StudentName.ToLower()) || x.User.LastName.ToLower().Contains(item.ToLower()));
                }
                query = query.Where(predicate);
            }

            if (GroupDefinitionId.HasValue)
            {
                query = query.Where(x => x.GroupDefinitionId == GroupDefinitionId);
            }

            if (Category.HasValue)
            {
                query = query.Where(x => x.Category == Category);
            }
            count = query.Count();

            return query.Include(x => x.GroupDefinition).Include(x => x.User).Skip((PageNumber - 1) * PageSize)
              .Take(PageSize)
              .AsNoTracking()
              .ToList();
        }
    }
}
