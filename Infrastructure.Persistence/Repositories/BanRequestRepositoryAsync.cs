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
    public class BanRequestRepositoryAsync : GenericRepositoryAsync<BanRequest>, IBanRequestRepositoryAsync
    {
        private readonly DbSet<BanRequest> _banrequests;


        public BanRequestRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _banrequests = dbContext.Set<BanRequest>();

        }
    }

}
