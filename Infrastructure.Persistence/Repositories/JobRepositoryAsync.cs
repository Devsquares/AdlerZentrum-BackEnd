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
    public class JobRepositoryAsync : GenericRepositoryAsync<Job>, IJobRepositoryAsync
    {
        private readonly DbSet<Job> _jobs;


        public JobRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _jobs = dbContext.Set<Job>();

        }
    }

}
