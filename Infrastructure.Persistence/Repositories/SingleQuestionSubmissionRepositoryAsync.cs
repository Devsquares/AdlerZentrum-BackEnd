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
    public class SingleQuestionSubmissionRepositoryAsync : GenericRepositoryAsync<SingleQuestionSubmission>, ISingleQuestionSubmissionRepositoryAsync
    {
        private readonly DbSet<SingleQuestionSubmission> _singlequestionsubmissions;


        public SingleQuestionSubmissionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _singlequestionsubmissions = dbContext.Set<SingleQuestionSubmission>();

        }
    }

}
