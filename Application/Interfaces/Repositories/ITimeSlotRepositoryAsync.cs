using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface ITimeSlotRepositoryAsync : IGenericRepositoryAsync<TimeSlot>
    {
        void DeleteDetails(int TimeSlotId);
        void AddDetails(List<TimeSlotDetails> list);
    }
}
