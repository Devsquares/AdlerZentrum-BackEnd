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
    public class BugRepositoryAsync : GenericRepositoryAsync<Bug>, IBugRepositoryAsync
    {
        private readonly DbSet<Bug> _bugs;


        public BugRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _bugs = dbContext.Set<Bug>();

        }
    }

}
