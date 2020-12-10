using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class GroupInstance : AuditableBaseEntity
    {
        public GroupInstance()
        {
            Students = new HashSet<GroupInstanceStudents>();
        }
        public int GroupDefinitionId { get; set; }
        public virtual GroupDefinition GroupDefinition { get; set; }
        public int Serail { get; set; }
        public int? Status { get; set; }
        public ICollection<GroupInstanceStudents> Students { get; set; }
    }
}
