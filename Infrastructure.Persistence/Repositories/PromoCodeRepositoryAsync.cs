using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class PromoCodeRepositoryAsync : GenericRepositoryAsync<PromoCode>, IPromoCodeRepositoryAsync
    {
        private readonly DbSet<PromoCode> promoCodes;

        public PromoCodeRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            promoCodes = dbContext.Set<PromoCode>();
        }

        public bool CheckPromoCode(string name)
        {
            var data = promoCodes.Where(x => x.Name == name).ToList();
            if (data.Count() > 0) { return true; }
            else { return false; }
        }

        public PromoCode GetByName(string name)
        {
            var data = promoCodes.Where(x => x.Name == name).ToList();
            return data.FirstOrDefault();
        }
    }
}
