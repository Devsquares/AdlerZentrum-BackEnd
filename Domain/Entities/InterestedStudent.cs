using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class InterestedStudent : AuditableBaseEntity
    {
        public int PromoCodeId { get; set; }
        public virtual PromoCode PromoCode { get; set; }
        public string StudentId { get; set; }
        public virtual ApplicationUser Student { get; set; }
        public int GroupDefinitionId { get; set; }
        public virtual GroupDefinition GroupDefinition { get; set; }
        public bool IsPlacementTest { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
