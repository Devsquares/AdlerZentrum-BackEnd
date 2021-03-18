using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class PlacementRelease : AuditableBaseEntity
    {
        public int TestId { get; set; }
        public Test Test { get; set; }
        public DateTime RelaeseDate { get; set; }
        public bool Cancel { get; set; }
    }
}
