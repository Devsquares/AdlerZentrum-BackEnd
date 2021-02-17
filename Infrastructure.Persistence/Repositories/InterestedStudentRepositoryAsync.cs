using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Models;
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
    public class InterestedStudentRepositoryAsync : GenericRepositoryAsync<InterestedStudent>, IInterestedStudentRepositoryAsync
    {
        private readonly DbSet<InterestedStudent> _interestedstudents;


        public InterestedStudentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _interestedstudents = dbContext.Set<InterestedStudent>();

        }

        public InterestedStudent GetByStudentId(string studentId, int groupDefinitionId)
        {
            return _interestedstudents.Where(x => x.StudentId == studentId && x.GroupDefinitionId == groupDefinitionId).FirstOrDefault();
        }

        public List<StudentsModel> GetByGroupDefinitionId( int groupDefinitionId)
        {
            var interestedStudents =  _interestedstudents.Include(x=>x.Student)
                .Include(x => x.GroupDefinition)
                .Include(x => x.PromoCodeInstance.PromoCode)
                .Where(x =>  x.GroupDefinitionId == groupDefinitionId)
                .Select(x=>new StudentsModel(){ 
                    StudentId = x.Student.Id,
                    StudentName =$"{x.Student.FirstName} {x.Student.LastName}",
                    PromoCodeId = x.PromoCodeInstance.PromoCode.Id,
                    PromoCodeName = x.PromoCodeInstance.PromoCode.Name,
                    isPlacementTest = x.IsPlacementTest,
                    ProfilePhoto = x.Student.Profilephoto,
                    PromoCodeInstanceId = x.PromoCodeInstanceId,
                    PromoCodeValue = x.PromoCodeInstance.PromoCode.Value
                }).ToList();
            return interestedStudents;
        }

        public int GetCountOfStudentsByGroupDefinitionId(int groupDefinitionId)
        {
            return _interestedstudents.Where(x => x.GroupDefinitionId == groupDefinitionId).Count();
        }

        public List<InterestedStudent> GetListByGroupDefinitionId(int groupDefinitionId)
        {
            return _interestedstudents.AsNoTracking().Include(x => x.Student)
                .Include(x => x.PromoCodeInstance.PromoCode)
                .Where(x => x.GroupDefinitionId == groupDefinitionId).OrderBy(x=>x.RegisterDate).ToList();
        }

        public async Task ADDList(List<InterestedStudent> students)
        {
            await _interestedstudents.AddRangeAsync(students);
        }
    }


}
