using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class GroupInstanceModel
    {
        public int GroupDefinitionId { get; set; }
        public DateTime GroupDefinitionStartDate { get; set; }
        public DateTime GroupDefinitionEndDate { get; set; }
        public DateTime? GroupDefinitionFinalTestDate { get; set; }
        public int GroupInstanceId { get; set; }
        public GroupInstance GroupInstance { get; set; }
        public string Serial { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsCurrent { get; set; } = false;
        public TimeSlot TimeSlots { get; set; }
        public Sublevel sublevel { get; set; }
        public double AchievedScore { get; set; }
        public bool Succeeded { get; set; }
        public bool IsDefault { get; set; }

    }
}
