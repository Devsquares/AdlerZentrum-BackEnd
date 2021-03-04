using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class AdlerCardSubmissionRepositoryAsync : GenericRepositoryAsync<AdlerCardSubmission>, IAdlerCardSubmissionRepositoryAsync
    {
        private readonly DbSet<AdlerCardSubmission> _adlercardsubmissions;


        public AdlerCardSubmissionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _adlercardsubmissions = dbContext.Set<AdlerCardSubmission>();

        }

        public AdlerCardSubmission GetAdlerCardForStudent(string studentId,int adlercardId)
        {
            return _adlercardsubmissions.Include(x => x.AdlerCard.Question).Include(x => x.AdlerCard.Level).Where(x => x.AdlerCardId == adlercardId && x.StudentId == studentId).FirstOrDefault();
        }
    }

}
