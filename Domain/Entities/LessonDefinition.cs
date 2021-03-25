using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class LessonDefinition : AuditableBaseEntity
    {
        public int SublevelId { get; set; }
        public virtual Sublevel Sublevel { get; set; }
        public int Order { get; set; }
        public bool LastLesson { get; set; }
    }
}
