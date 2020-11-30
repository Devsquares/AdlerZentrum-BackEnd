using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TeacherGroupInstanceAssignment : AuditableBaseEntity
    {
        public string TeacherId { get; set; }
        public virtual ApplicationUser Teacher { get; set; }

        public int GroupInstanceId { get; set; }
        public virtual GroupInstance GroupInstance { get; set; }
        public bool IsDefault { get; set; }
    }
}
