using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ChoiceAnswer : AuditableBaseEntity
    {
        public string Name { get; set; }
        public int SingleQuestionId { get; set; }
        public virtual SingleQuestion SingleQuestion { get; set; }
    }
}
