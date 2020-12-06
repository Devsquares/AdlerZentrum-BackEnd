using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    class TimeSlotRepositoryAsync : GenericRepositoryAsync<TimeSlot>, ITimeSlotRepositoryAsync
    {
        private readonly DbSet<TimeSlot> timeSlots;

        public TimeSlotRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            timeSlots = dbContext.Set<TimeSlot>();
        }
    }
}
