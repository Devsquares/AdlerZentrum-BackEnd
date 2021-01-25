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
    public class DisqualificationRequestRepositoryAsync : GenericRepositoryAsync<DisqualificationRequest>, IDisqualificationRequestRepositoryAsync
    {
        private readonly DbSet<DisqualificationRequest> _disqualificationrequests;


        public DisqualificationRequestRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _disqualificationrequests = dbContext.Set<DisqualificationRequest>();

        }
    }

}
