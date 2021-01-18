using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Sublevel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public int LevelId { get; set; }
        public virtual Level Level { get; set; }
        public int NumberOflessons { get; set; }
        public string Color { get; set; }
        public List<LessonDefinition> LessonDefinitions { get; set; }
    }
}
