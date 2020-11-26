using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TeacherGroupInstanceAssignment : AuditableBaseEntity
    {
        public Account Teacher { get; set; }
    }
}
