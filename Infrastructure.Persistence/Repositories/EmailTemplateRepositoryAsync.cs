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
    public class EmailTemplateRepositoryAsync : GenericRepositoryAsync<EmailTemplate>, IEmailTemplateRepositoryAsync
    {
        private readonly DbSet<EmailTemplate> _emailtemplates;


        public EmailTemplateRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _emailtemplates = dbContext.Set<EmailTemplate>();

        }

        public List<EmailTemplate> GetEmailTemplateByEmailTypeId(int emailTypeId)
        {
            return _emailtemplates.Where(x => x.EmailTypeId == emailTypeId).ToList();
        }
    }

}
