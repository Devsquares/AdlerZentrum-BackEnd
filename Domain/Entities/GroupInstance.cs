using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class GroupInstance : AuditableBaseEntity
    {
        public int GroupDefinitionId { get; set; }
        public GroupDefinition GroupDefinition { get; set; }
        public int Serail { get; set; }
        public int Status { get; set; }
    }
}
