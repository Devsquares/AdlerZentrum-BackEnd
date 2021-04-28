using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class DuplicateException : AuditableBaseEntity
    { 
        [Required]
        public string Email { get; set; }
     
    }
}
