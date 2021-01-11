using Domain.Common;

namespace Domain.Entities
{
    public class AbsenceReminder : AuditableBaseEntity
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; } 
    }
}
