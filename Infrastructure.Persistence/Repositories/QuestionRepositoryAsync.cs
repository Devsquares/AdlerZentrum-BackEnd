using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
