using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class PromoCode : AuditableBaseEntity
    {
        public string Name { get; set; }
        public int? Status { get; set; }
        public int? GroupId { get; set; }
    }
}
