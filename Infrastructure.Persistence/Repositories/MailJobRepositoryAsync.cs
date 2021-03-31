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
    public class MailJobRepositoryAsync : GenericRepositoryAsync<MailJob>, IMailJobRepositoryAsync
    {
        private readonly DbSet<MailJob> _jobs;


        public MailJobRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _jobs = dbContext.Set<MailJob>();
        }
    }

}
