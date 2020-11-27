using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Domain.Entities
{
    public class TimeSlot : AuditableBaseEntity
    {
        public string Name { get; set; }
        public int Status { get; set; }
        
        public virtual ICollection<TimeSlotDetails> TimeSlotDetails { get; set; }
    }
}
