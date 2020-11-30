
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class GroupdefinitionLessonTest : AuditableBaseEntity
    {
        public int GroupDefinitionId { get; set; }
        public virtual GroupDefinition GroupDefinition { get; set; }
        public int LessonDefinitionId { get; set; }
        public virtual LessonDefinition LessonDefinition { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
    }
}
