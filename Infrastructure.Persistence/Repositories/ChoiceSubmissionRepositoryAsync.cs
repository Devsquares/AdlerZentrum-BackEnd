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
    public class ChoiceSubmissionRepositoryAsync : GenericRepositoryAsync<ChoiceSubmission>, IChoiceSubmissionRepositoryAsync
    {
        private readonly DbSet<ChoiceSubmission> _choicesubmissions;


        public ChoiceSubmissionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _choicesubmissions = dbContext.Set<ChoiceSubmission>();

        }
    }

}
