using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITimeSlotRepositoryAsync : IGenericRepositoryAsync<TimeSlot>
    {
        void DeleteDetails(int TimeSlotId);
        Task AddDetails(List<TimeSlotDetails> list);
    }
}
