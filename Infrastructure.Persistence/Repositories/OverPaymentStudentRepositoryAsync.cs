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

        public object GetByGroupDefinitionId(int groupDefinitionId)
        {
            var overpaymentstudents = _overpaymentstudents.Include(x => x.Student)
                .Include(x => x.GroupDefinition)
                .Where(x => x.GroupDefinitionId == groupDefinitionId)
                .Select(x => new
                {
                    StudentId = x.Student.Id,
                    StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                    IsPlacementTest = x.IsPlacementTest
                }).ToList();
            return overpaymentstudents;
        }

        public int GetCountOfStudentsByGroupDefinitionId(int groupDefinitionId)
        {
            return _overpaymentstudents.Where(x => x.GroupDefinitionId == groupDefinitionId).Count();
        }

        public List<OverPaymentStudent> GetDefaultListByGroupDefinitionId(int groupDefinitionId)
        {
            return _overpaymentstudents.Include(x => x.Student)
                .Where(x => x.GroupDefinitionId == groupDefinitionId && x.IsPlacementTest == false).OrderBy(x => x.RegisterDate).ToList();

        }
        public List<OverPaymentStudent> GetPlacementTestListByGroupDefinitionId(int groupDefinitionId)
        {
            return _overpaymentstudents.Include(x => x.Student)
                .Where(x => x.GroupDefinitionId == groupDefinitionId && x.IsPlacementTest == true).OrderBy(x => x.RegisterDate).ToList();

        }

        public async Task ADDList(List<OverPaymentStudent> students)
        {
            await _overpaymentstudents.AddRangeAsync(students);
        }
    }

}
