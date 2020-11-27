using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Common
{
    public abstract class AuditableBaseEntity
    {
        public virtual int Id { get; set; }

        [MaxLength(256)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(256)]
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
