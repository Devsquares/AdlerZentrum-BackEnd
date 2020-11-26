using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class GroupCondition : AuditableBaseEntity
    {
        public int Status { get; set; }
    }
}
