using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Pricing : AuditableBaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Status { get; set; }
    }
}
