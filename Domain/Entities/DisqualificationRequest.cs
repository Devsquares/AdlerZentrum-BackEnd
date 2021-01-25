using Domain.Common;

namespace Domain.Entities
{
    public class DisqualificationRequest : AuditableBaseEntity
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public string Comment { get; set; }
        public int DisqualificationRequestStatus { get; set; }
    }
}
