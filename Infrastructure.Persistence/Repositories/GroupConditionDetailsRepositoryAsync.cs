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
    public class GroupConditionDetailsRepositoryAsync : GenericRepositoryAsync<GroupConditionDetail>, IGroupConditionDetailsRepositoryAsync
    {
        private readonly DbSet<GroupConditionDetail> _groupconditiondetailss;
        private readonly ApplicationDbContext _dbContext;

        public GroupConditionDetailsRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _groupconditiondetailss = dbContext.Set<GroupConditionDetail>();
            _dbContext = dbContext;

        }

        public List<GroupConditionDetail> GetByGroupConditionId(int configCondID)
        {
            return _groupconditiondetailss.Where(x => x.GroupConditionId == configCondID).ToList();
        }

        public List<GroupConditionDetail> GetByGroupConditionIds(List<int> configCondIDs)
        {
            return _groupconditiondetailss.Where(x => configCondIDs.Contains(x.GroupConditionId)).ToList();
        }

        //public List<GetAllGroupConditionViewModel> GetAllByGroupConditionDetailId(List<int> groupConditionids)
        //{
        //    _groupconditiondetailss
        //       .Include(x => x.GroupCondition)
        //       .Join(_dbContext.groupConditionPromoCodes,
        //       gcd => gcd.Id,
        //       gcpc => gcpc.GroupConditionDetailsId,
        //       (gcd,gcpc)=> new { gcd, gcpc })
        //       .Where(x => groupConditionids.Contains(x.gcd.GroupConditionId))
        //       .Select(x => new GetAllGroupConditionViewModel()
        //       {
        //           Id = x.gcd.GroupConditionId,
        //           Status = x.gcd.GroupCondition.Status,
        //           NumberOfSolts = x.gcd.GroupCondition.NumberOfSolts,
        //           NumberOfSlotsWithPlacementTest = x.gcd.GroupCondition.NumberOfSlotsWithPlacementTest,
        //           PromoCodes = x.gcpc
        //       })
        //       .ToList();
        //}
    }

}
