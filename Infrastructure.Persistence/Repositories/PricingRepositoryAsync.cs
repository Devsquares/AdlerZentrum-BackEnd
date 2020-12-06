using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    class PricingRepositoryAsync : GenericRepositoryAsync<Pricing>, IPricingRepositoryAsync
    {
        private readonly DbSet<Pricing> Pricings;

        public PricingRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            Pricings = dbContext.Set<Pricing>();
        }
    }
}
