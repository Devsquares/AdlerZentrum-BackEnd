using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs 
{
    public class GetAllTimeSlotsViewModel
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        public int? Status { get; set; }
        public virtual ICollection<TimeSlotDetails> TimeSlotDetails { get; set; }

    }
}
