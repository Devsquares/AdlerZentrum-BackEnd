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
    public class AdlerCardsBundleRepositoryAsync : GenericRepositoryAsync<AdlerCardsBundle>, IAdlerCardsBundleRepositoryAsync
    {
        private readonly DbSet<AdlerCardsBundle> _adlercardsbundles;


        public AdlerCardsBundleRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _adlercardsbundles = dbContext.Set<AdlerCardsBundle>();

        }
    }

}
