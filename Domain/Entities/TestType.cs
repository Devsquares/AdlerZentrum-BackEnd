using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TestType : AuditableBaseEntity
    {
        public string Name { get; set; }
    }
}
