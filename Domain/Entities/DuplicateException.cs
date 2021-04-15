using Domain.Common;

namespace Domain.Entities
{
    public class DuplicateException : AuditableBaseEntity
    { 
        public string Email { get; set; }
     
    }
}
