using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    class DuplicateExceptionRepositoryAsync : GenericRepositoryAsync<DuplicateException>, IDuplicateExceptionRepositoryAsync
    {
        private readonly DbSet<DuplicateException> duplicateExceptions;

        public DuplicateExceptionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            duplicateExceptions = dbContext.Set<DuplicateException>();
        }

        public DuplicateException GetByEmail(string email)
        {
            return duplicateExceptions.Where(x => x.Email == email).FirstOrDefault();
        }
        public bool check(string email)
        {
            return duplicateExceptions.Where(x => x.Email == email).Any();
        }
    }
}
