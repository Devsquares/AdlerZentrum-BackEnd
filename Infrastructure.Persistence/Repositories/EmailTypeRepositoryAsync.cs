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
    public class EmailTypeRepositoryAsync : GenericRepositoryAsync<EmailType>, IEmailTypeRepositoryAsync
    {
        private readonly DbSet<EmailType> _emailtypes;


        public EmailTypeRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _emailtypes = dbContext.Set<EmailType>();

        }
    }

}
