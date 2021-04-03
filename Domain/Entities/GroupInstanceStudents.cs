using Domain.Common;

namespace Domain.Entities
{
    public class GroupInstanceStudents : AuditableBaseEntity
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public int GroupInstanceId { get; set; }
        public GroupInstance GroupInstance { get; set; }
        public bool IsDefault { get; set; }
        public bool IsPlacementTest { get; set; }
        public int? PromoCodeInstanceId { get; set; }
        public virtual PromoCodeInstance PromoCodeInstance { get; set; }
        public double AchievedScore { get; set; }
        public bool Succeeded { get; set; }
        public bool IsEligible { get; set; }
        public bool Disqualified { get; set; }
        public string DisqualifiedComment { get; set; }
        public string DisqualifiedUserId {get;set;}
    }
}
