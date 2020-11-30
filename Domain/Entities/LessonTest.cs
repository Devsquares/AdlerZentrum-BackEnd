using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class LessonTest : AuditableBaseEntity
    {
        public int LessonDefinitionId { get; set; }
        public virtual LessonDefinition LessonDefinition { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
    }
}
