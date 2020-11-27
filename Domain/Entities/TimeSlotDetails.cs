using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TimeSlotDetails : AuditableBaseEntity
    {
        public int TimeSlotId { get; set; }
        public virtual TimeSlot TimeSlot { get; set; }
        public int WeekDay { get; set; }

        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
    }
}
