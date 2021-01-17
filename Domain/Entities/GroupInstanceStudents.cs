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
        public int? PromoCodeId { get; set; }
        public virtual PromoCode PromoCode { get; set; }

    }
}
