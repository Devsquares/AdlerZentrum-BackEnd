using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class OverPaymentStudent : AuditableBaseEntity
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public int GroupDefinitionId { get; set; }
        public GroupDefinition GroupDefinition { get; set; }
        public bool IsPlacementTest { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
