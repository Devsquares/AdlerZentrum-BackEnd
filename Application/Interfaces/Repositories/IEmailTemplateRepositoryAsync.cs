using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IEmailTemplateRepositoryAsync : IGenericRepositoryAsync<EmailTemplate>
    {
        List<EmailTemplate> GetEmailTemplateByEmailTypeId(int emailTypeId);
    }
}
