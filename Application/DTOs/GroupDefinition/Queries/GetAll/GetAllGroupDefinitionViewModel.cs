using Domain.Entities;
using System; 

namespace Application.DTOs
{
    public class GetAllGroupDefinitionViewModel
    {
        public int Id { get; set; }
        public int SubLevelId { get; set; }

        public virtual Sublevel Sublevel { get; set; }

        public int TimeSlotId { get; set; }

        public virtual TimeSlot TimeSlot { get; set; }

        public int PricingId { get; set; }

        public Pricing Pricing { get; set; }

        public int GroupConditionId { get; set; }

        public GroupCondition GroupCondition { get; set; }

        public double Discount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? FinalTestDate { get; set; }

        public int MaxInstances { get; set; }

        public int Serial { get; set; }

        public int? Status { get; set; }

        public bool IsInterested { get; set; }

    }
}
