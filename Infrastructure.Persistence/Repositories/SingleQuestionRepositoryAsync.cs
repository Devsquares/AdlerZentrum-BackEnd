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
    public class SingleQuestionRepositoryAsync : GenericRepositoryAsync<SingleQuestion>, ISingleQuestionRepositoryAsync
    {
        private readonly DbSet<SingleQuestion> _singleQuestion;

        public SingleQuestionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _singleQuestion = dbContext.Set<SingleQuestion>();
        }

        public async Task<IReadOnlyList<SingleQuestion>> GetPagedReponseAsync(int pageNumber, int pageSize, int typeId)
        {
            return await _singleQuestion.Where(x => x.SingleQuestionType == typeId)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .AsNoTracking()
           .ToListAsync();
        }
    }
}
