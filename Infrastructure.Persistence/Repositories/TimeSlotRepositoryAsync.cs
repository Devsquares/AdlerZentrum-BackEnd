using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    class TimeSlotRepositoryAsync : GenericRepositoryAsync<TimeSlot>, ITimeSlotRepositoryAsync
    {
        private readonly DbSet<TimeSlot> timeSlots;
        private readonly DbSet<TimeSlotDetails> timeSlotDetails;
        private readonly ApplicationDbContext _dbContext;
        public TimeSlotRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            timeSlots = dbContext.Set<TimeSlot>();
            timeSlotDetails = dbContext.Set<TimeSlotDetails>();
            _dbContext = dbContext;
        }

        public void DeleteDetails(int timeSlotId)
        {
            var data = timeSlotDetails.Where(x => x.TimeSlotId == timeSlotId);
            timeSlotDetails.RemoveRange(data);
        }

        public async void AddDetails(List<TimeSlotDetails> list)
        {
            timeSlotDetails.AddRange(list);
            _dbContext.SaveChanges();
        }

        public override async Task<TimeSlot> GetByIdAsync(int id)
        {
            return await timeSlots.Include(x => x.TimeSlotDetails).Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
