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
    public class SingleQuestionRepositoryAsync : GenericRepositoryAsync<SingleQuestion>, ISingleQuestionRepositoryAsync
    {
        private readonly DbSet<SingleQuestion> _singleQuestion;

        public SingleQuestionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _singleQuestion = dbContext.Set<SingleQuestion>();
        }
    }
}
