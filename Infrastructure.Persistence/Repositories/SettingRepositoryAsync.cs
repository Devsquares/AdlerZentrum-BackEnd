using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class SettingRepositoryAsync : GenericRepositoryAsync<Setting>, ISettingRepositoryAsync
    {
        private readonly DbSet<Setting> _settings;

        public SettingRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _settings = dbContext.Set<Setting>();
        }
    }
}
