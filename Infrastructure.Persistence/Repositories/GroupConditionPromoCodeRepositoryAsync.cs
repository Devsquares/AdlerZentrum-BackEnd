using Application.DTOs;
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

namespace Infrastructure.Persistence.Repositories
{
    public class GroupConditionPromoCodeRepositoryAsync : GenericRepositoryAsync<GroupConditionPromoCode>, IGroupConditionPromoCodeRepositoryAsync
    {
        private readonly DbSet<GroupConditionPromoCode> _groupconditionpromocodes;
        ApplicationDbContext _dbContext;
        private readonly DbSet<GroupInstanceStudents> _groupInstanceStudents;
        private readonly DbSet<InterestedStudent> _interestedStudent;
        private readonly DbSet<PromoCodeInstance> _promoCodeInstance;
        public GroupConditionPromoCodeRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _groupconditionpromocodes = dbContext.Set<GroupConditionPromoCode>();
            _groupInstanceStudents = dbContext.Set<GroupInstanceStudents>();
            _interestedStudent = dbContext.Set<InterestedStudent>();
            _dbContext = dbContext;
            _promoCodeInstance = dbContext.Set<PromoCodeInstance>();
        }

        public List<GroupConditionPromoCode> GetByGroupConditionDetailId(List<GroupConditionDetail> groupConditionDetails)
        {
            var ids = groupConditionDetails.Select(x => x.Id).ToList();
            return _groupconditionpromocodes
                .Include(x => x.PromoCode)
                .Include(x => x.GroupConditionDetails)
                .Where(x => ids.Contains(x.GroupConditionDetailsId)).ToList();
        }

        public bool CheckPromoCodeCountInGroupInstance(int groupInstanceId, int promocodeInstanceId, List<GroupInstanceStudents> groupInstanceStudentsList = null,string promoCodeKey = null)
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
            var studentsGroup = new List<PromoCodeCountModel>();
            if (groupInstanceStudentsList != null && groupInstanceStudentsList.Count>0) // from list not database
            {
                studentsGroup = groupInstanceStudentsList.GroupBy(x => x.PromoCodeInstance.PromoCodeId) // modified
                   .Select(x => new PromoCodeCountModel() { promocodeId = x.Key, count = x.Count() }).ToList();
            }
            //else if (studentsModelList != null && studentsModelList.Count > 0) // from list not database
            //{
            //    studentsGroup = studentsModelList.GroupBy(x => x.PromoCodeId)
            //       .Select(x => new PromoCodeCountModel()  { promocodeId = x.Key, count = x.Count() }).ToList();
            //}
            else
            {
                studentsGroup = _groupInstanceStudents.Where(x => x.GroupInstanceId == groupInstanceId && x.PromoCodeInstanceId != null)
             .GroupBy(x => x.PromoCodeInstance.PromoCodeId)//Modified
             .Select(x => new PromoCodeCountModel() { promocodeId = x.Key, count = x.Count() }).ToList();
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
            var promocodeObject = _promoCodeInstance.Include(x => x.PromoCode).Where(x => x.Id == promocodeInstanceId).FirstOrDefault();
            if(promocodeObject == null)
            {
                throw new Exception("PromoCode Not Found");
            }
            var studentpromocount = studentsGroup.Where(x => x.promocodeId == promocodeObject.PromoCodeId).FirstOrDefault();

            foreach (var item in validDetails)
            {
                var validpromo = item.Where(x => x.PromoCodeId == promocodeObject.PromoCodeId).FirstOrDefault();
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

        public bool CheckPromoCodeInGroupDefinitionGeneral(int groupDefinitionId, int promocodeInstanceId, string promoCodeKey = null)
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
            var promocodeObject = _promoCodeInstance.Include(x => x.PromoCode).Where(x => x.Id == promocodeInstanceId).FirstOrDefault();
            if (promocodeObject == null)
            {
                throw new Exception("PromoCode Not Found");
            }
            var interestedStudentsCount = _interestedStudent.Include(x=>x.PromoCodeInstance).Where(x => x.GroupDefinitionId == groupDefinitionId && x.PromoCodeInstance.PromoCodeId == promocodeObject.PromoCodeId).Count();
            foreach (var item in GroupConditionDetails)
            {
                var validpromo = item.Where(x => x.PromoCodeId == promocodeObject.PromoCodeId).FirstOrDefault();
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

        public List<IGrouping<int, GroupConditionPromoCode>> GetAllByGroupCondition(int groupConditionId)
        {
            var proomcodes = _groupconditionpromocodes.Include(x=>x.PromoCode)
                                                        .Where(x => x.GroupConditionDetails.GroupConditionId == groupConditionId).ToList();
                                                        
            var groupedpromocodes = proomcodes.GroupBy(x => x.GroupConditionDetailsId).ToList();
            return groupedpromocodes;
        }

    }

}
