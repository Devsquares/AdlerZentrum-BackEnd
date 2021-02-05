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
    public class SingleQuestionSubmissionRepositoryAsync : GenericRepositoryAsync<SingleQuestionSubmission>, ISingleQuestionSubmissionRepositoryAsync
    {
        private readonly DbSet<SingleQuestionSubmission> _singlequestionsubmissions;


        public SingleQuestionSubmissionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _singlequestionsubmissions = dbContext.Set<SingleQuestionSubmission>();

        }
        
        public async Task<IReadOnlyList<SingleQuestionSubmission>> GetByTestInstanceIdAsync(int TestInstanceId)
        {
            return await _singlequestionsubmissions
              .Include(x => x.Choices)
              .Where(x => x.TestInstanceId == TestInstanceId).ToListAsync();
        }
    }

}
