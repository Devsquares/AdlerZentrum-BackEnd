using Application.DTOs;
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
    public class GroupConditionPromoCodeRepositoryAsync : GenericRepositoryAsync<GroupConditionPromoCode>, IGroupConditionPromoCodeRepositoryAsync
    {
        private readonly DbSet<GroupConditionPromoCode> _groupconditionpromocodes;
        ApplicationDbContext _dbContext;
        private readonly DbSet<GroupInstanceStudents> _groupInstanceStudents;
        private readonly DbSet<InterestedStudent> _interestedStudent;
        public GroupConditionPromoCodeRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _groupconditionpromocodes = dbContext.Set<GroupConditionPromoCode>();
            _groupInstanceStudents = dbContext.Set<GroupInstanceStudents>();
            _interestedStudent = dbContext.Set<InterestedStudent>();
            _dbContext = dbContext;
        }

        public List<GroupConditionPromoCode> GetByGroupConditionDetailId(List<GroupConditionDetail> groupConditionDetails)
        {
            var ids = groupConditionDetails.Select(x => x.Id).ToList();
            return _groupconditionpromocodes
                .Include(x => x.PromoCode)
                .Include(x => x.GroupConditionDetails)
                .Where(x => ids.Contains(x.GroupConditionDetailsId)).ToList();
        }

        public bool CheckPromoCodeCountInGroupInstance(int groupInstanceId, int promocodeId, List<GroupInstanceStudents> groupInstanceStudentsList = null)
        {
            bool canApply = false;
            var promocodes = _groupconditionpromocodes.Include(x => x.GroupConditionDetails)
                .Join(_dbContext.GroupInstances,
                gcpc => gcpc.GroupConditionDetails.GroupConditionId,
                gi => gi.GroupDefinition.GroupConditionId,
                (gcpc, gi) => new { gcpc, gi })
                .Where(x => x.gi.Id == groupInstanceId)
                .Select(x => x.gcpc).ToList();
            var GroupConditionDetails = promocodes.GroupBy(x => x.GroupConditionDetailsId).ToList();
            //var studentsGroup = _groupInstanceStudents.Where(x => x.GroupInstanceId == groupInstanceId).ToList();

            var studentsGroup = _groupInstanceStudents.Where(x => x.GroupInstanceId == groupInstanceId && x.PromoCodeId != null)
               .GroupBy(x => x.PromoCodeId)
               .Select(x => new { promocodeId = x.Key, count = x.Count() }).ToList();
            if (groupInstanceStudentsList != null && groupInstanceStudentsList.Count>0) // from list not database
            {
                studentsGroup = groupInstanceStudentsList.GroupBy(x => x.PromoCodeId)
                   .Select(x => new { promocodeId = x.Key, count = x.Count() }).ToList();
            }
            // List<int> promocodsIDS = new List<int>();
            //foreach (var item in studentsGroup)
            //{
            //    promocodsIDS.Add(item.promocodeId.Value);
            //}

            // List<object> promovalid = new List<object>();
            List<IGrouping<int, GroupConditionPromoCode>> validDetails = new List<IGrouping<int, GroupConditionPromoCode>>();
            bool isvalidDetails = false;
            foreach (var item in GroupConditionDetails)
            {
                isvalidDetails = true;
                foreach (var item2 in studentsGroup)
                {
                    var detail = item.Where(x => x.PromoCodeId == item2.promocodeId && item2.count <= x.Count).FirstOrDefault();
                    if (detail == null)
                    {
                        isvalidDetails = false;
                        break;
                    }
                }
                if (isvalidDetails)
                {
                    validDetails.Add(item);
                }

            }
            var studentpromocount = studentsGroup.Where(x => x.promocodeId == promocodeId).FirstOrDefault();

            foreach (var item in validDetails)
            {
                var validpromo = item.Where(x => x.PromoCodeId == promocodeId).FirstOrDefault();
                if (studentpromocount == null && validpromo == null)
                {
                    canApply = false;
                }
                else if (studentpromocount == null && validpromo != null) // first student
                {
                    canApply = true;
                    break;
                }
                else if (studentpromocount != null && validpromo != null && studentpromocount.count < validpromo.Count)
                {
                    canApply = true;
                    break;
                }
            }
            return canApply;
        }

        public bool CheckPromoCodeInGroupDefinitionGeneral(int groupDefinitionId, int promocodeId)
        {
            bool canApply = false;
            var promocodes = _groupconditionpromocodes.Include(x => x.GroupConditionDetails)
                .Join(_dbContext.GroupInstances,
                gcpc => gcpc.GroupConditionDetails.GroupConditionId,
                gi => gi.GroupDefinition.GroupConditionId,
                (gcpc, gi) => new { gcpc, gi })
                .Where(x => x.gi.GroupDefinitionId == groupDefinitionId)
                .Select(x => x.gcpc).ToList();
            var GroupConditionDetails = promocodes.GroupBy(x => x.GroupConditionDetailsId).ToList();

            var interestedStudentsCount = _interestedStudent.Where(x => x.GroupDefinitionId == groupDefinitionId && x.PromoCodeId == promocodeId).Count();
            foreach (var item in GroupConditionDetails)
            {
                var validpromo = item.Where(x => x.PromoCodeId == promocodeId).FirstOrDefault();
                if (validpromo == null)
                {
                    canApply = false;
                }
                else if (validpromo != null && interestedStudentsCount < validpromo.Count)
                {
                    canApply = true;
                    break;
                }
            }
            return canApply;
        }

    }

}
