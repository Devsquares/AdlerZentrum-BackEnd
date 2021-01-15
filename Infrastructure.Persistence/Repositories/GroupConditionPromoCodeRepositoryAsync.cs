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


        public GroupConditionPromoCodeRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _groupconditionpromocodes = dbContext.Set<GroupConditionPromoCode>();

        }

        public List<GroupConditionPromoCode> GetByGroupConditionDetailId(List<GroupConditionDetail> groupConditionDetails)
        {
            var ids = groupConditionDetails.Select(x => x.Id).ToList();
            return _groupconditionpromocodes
                .Include(x=>x.PromoCode)
                .Include(x => x.GroupConditionDetails)
                .Where(x => ids.Contains(x.GroupConditionDetailsId)).ToList();
        }

        //public List<GetAllGroupConditionViewModel> GetAllByGroupConditionDetailId(List<int> groupConditionids)
        //{
        //     _groupconditionpromocodes
        //        .Include(x=>x.GroupConditionDetails.GroupCondition)
        //        .Where(x => groupConditionids.Contains(x.GroupConditionDetails.GroupConditionId))
        //        .Select( x=> new GetAllGroupConditionViewModel() { 
        //            Id = x.GroupConditionDetails.GroupConditionId,
        //            Status = x.GroupConditionDetails.GroupCondition.Status,
        //            NumberOfSolts = x.GroupConditionDetails.GroupCondition.NumberOfSolts,
        //            NumberOfSlotsWithPlacementTest= x.GroupConditionDetails.GroupCondition.NumberOfSlotsWithPlacementTest,
        //            PromoCodes = x.PromoCode
        //        })
        //        .ToList();
        //}
    }

}
