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
    public class QuestionRepositoryAsync : GenericRepositoryAsync<Question>, IQuestionRepositoryAsync
    {
        private readonly DbSet<Question> _questions;

        public QuestionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _questions = dbContext.Set<Question>();
        }

        public async Task<List<Question>> GetAllByTypeIdAsync(int questionTypeId, int pageNumber, int pageSize)
        {
            return await _questions.Include(x => x.SingleQuestions).ThenInclude(x => x.Choices)
                .Where(x => x.QuestionTypeId == questionTypeId).Include(x => x.SingleQuestions)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<Question>> GetAllByTypeIdNotUsedAsync(int questionTypeId, int pageNumber, int pageSize)
        {
            return await _questions.Include(x => x.SingleQuestions).ThenInclude(x => x.Choices)
                .Where(x => x.QuestionTypeId == questionTypeId && x.TestId == null).Include(x => x.SingleQuestions)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        }

        public int GetAllByTypeIdCountAsync(int questionTypeId)
        {
            return _questions.Where(x => x.QuestionTypeId == questionTypeId).ToList().Count();
        }

        public override Task<Question> GetByIdAsync(int id)
        {
            return _questions.Include(x => x.SingleQuestions)
                   .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Question>> GetByTestIdAsync(int id)
        {
            return await _questions.Where(x => x.TestId == id).ToListAsync();
        }

        public async Task<IReadOnlyList<Question>> GetAllAsync(int pageNumber, int pageSize, int? questionTypeId = null)
        {
            return await _questions.Include(x => x.SingleQuestions).ThenInclude(x => x.Choices)
                .Where(x => questionTypeId != null ? x.QuestionTypeId == questionTypeId : true).Include(x => x.SingleQuestions)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        }
    }
}
