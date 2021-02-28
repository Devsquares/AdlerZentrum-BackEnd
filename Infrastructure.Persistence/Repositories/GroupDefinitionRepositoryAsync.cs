using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class GroupDefinitionRepositoryAsync : GenericRepositoryAsync<GroupDefinition>, IGroupDefinitionRepositoryAsync
    {
        private readonly DbSet<GroupDefinition> groupDefinitions;
        private readonly DbSet<Sublevel> sublevels;
        private readonly int SERIAL_DIGITS = 4;
        public GroupDefinitionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            groupDefinitions = dbContext.Set<GroupDefinition>();
            sublevels = dbContext.Set<Sublevel>();
        }

        public new async Task<GroupDefinition> AddAsync(GroupDefinition entity)
        {
            await SetSerialNumberBeforeInsert(entity);
            await groupDefinitions.AddAsync(entity);
            return await base.AddAsync(entity);
        }

        public List<GroupDefinition> GetALL(int pageNumber, int pageSize, string subLevelName, out int totalCount,int? sublevelId=null)
        {
            var groupDefinitionsList = groupDefinitions.Include(x => x.GroupCondition).Include(x => x.Sublevel)
                .Where(x => (!string.IsNullOrEmpty(subLevelName) ? (x.Sublevel.Name.ToLower() == subLevelName.ToLower()) : true)
                        && (sublevelId !=null? x.SubLevelId== sublevelId.Value : true) ).ToList();
            totalCount = groupDefinitionsList.Count();
            groupDefinitionsList = groupDefinitionsList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return groupDefinitionsList;
        }

        public GroupDefinition GetById(int groupDefinitionId)
        {
            return groupDefinitions
            .Include(x => x.TimeSlot)
            .Include(x => x.GroupCondition)
            .Include(x => x.Sublevel)
            .Include(x => x.Pricing)
            .Where(x => x.Id == groupDefinitionId).FirstOrDefault();
        }

        private async Task SetSerialNumberBeforeInsert(GroupDefinition groupDefinition)
        {
            string serial = "";
            string sublevel,number;
            int count;
           
            if (groupDefinition.SubLevelId == 0)
                throw new ApiException("Error while generating the serial number for the new " +
                    "group definition. Sublevel couldn't be found.");
            sublevel = sublevels.Find(groupDefinition.SubLevelId).Name;
            sublevel = sublevel.Replace(".", "");

            count = await groupDefinitions.Where(x => x.Serial != null && x.Serial != "" && x.Serial.Length == 7 && x.SubLevelId == groupDefinition.SubLevelId).CountAsync();
            if (count == 0)
                number = "0".PadLeft(SERIAL_DIGITS, '0');
            else
                number = _findNextSerial(groupDefinition.SubLevelId);
            serial = sublevel + number;
            groupDefinition.Serial = serial;
        }

        private string _findNextSerial(int sublevelId)
        {
            string newSerial;
            int maxSerialInt, newSerialInt;

            maxSerialInt =  groupDefinitions.Where(x=> x.Serial != null && x.Serial != "" && x.Serial.Length == 7 && x.SubLevelId == sublevelId).ToList().Max(x => int.Parse(x.Serial.Substring(3,4)));
            newSerialInt = maxSerialInt + 1;
            newSerial = newSerialInt.ToString().PadLeft(SERIAL_DIGITS, '0');
            return newSerial;
        }
    }
}
