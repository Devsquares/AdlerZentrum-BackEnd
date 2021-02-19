using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class BugRepositoryAsync : GenericRepositoryAsync<Bug>, IBugRepositoryAsync
    {
        private readonly DbSet<Bug> _bugs;
        private readonly ApplicationDbContext _dbContext;


        public BugRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _bugs = _dbContext.Set<Bug>();

        }

        public async Task<IReadOnlyList<Bug>> GetPagedReponseAsync(int pageNumber, int pageSize, string type, string status, string priority)
        {
            return await _dbContext.Set<Bug>()
                .Where(b => b.Status != "Archived" && (status == null || b.Status == status) && (type == null || b.Type == type) && (priority == null || b.Priority == priority))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public int GetCount(string type, string status, string priority)
        {
            return _dbContext.Set<Bug>()
                .Where(b => b.Status != "Archived" && (status == null || b.Status == status) && (type == null || b.Type == type) && (priority == null || b.Priority == priority))
                .Count();
        }
    }
}
