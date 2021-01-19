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
    public class OverPaymentStudentRepositoryAsync : GenericRepositoryAsync<OverPaymentStudent>, IOverPaymentStudentRepositoryAsync
    {
        private readonly DbSet<OverPaymentStudent> _overpaymentstudents;


        public OverPaymentStudentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _overpaymentstudents = dbContext.Set<OverPaymentStudent>();

        }

        public OverPaymentStudent GetByStudentId(string studentId, int groupDefinitionId)
        {
            return _overpaymentstudents.Where(x => x.StudentId == studentId && x.GroupDefinitionId == groupDefinitionId).FirstOrDefault();
        }
    }

}
