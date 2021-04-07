using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Setting : AuditableBaseEntity
    {
        public double PlacementA1 { get; set; } 
        public double PlacementA2 { get; set; }
        public double PlacementB1 { get; set; }
        public double PlacementB2 { get; set; }
        public double PlacementC1 { get; set; }
    }
}
