using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class GetAllTimeSlotsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public ICollection<TimeSlotDetailsViewModel> TimeSlotDetails { get; set; }

    }
    public class TimeSlotDetailsViewModel
    {
        public int Id { get; set; }
        public int TimeSlotId { get; set; }
        public int WeekDay { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
    }
}
