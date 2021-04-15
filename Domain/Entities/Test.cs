
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Test : AuditableBaseEntity
    {
        public string Name { get; set; }
        public int TestDuration { get; set; }
        public int TestTypeId { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public int? LessonDefinitionId { get; set; }
        public LessonDefinition LessonDefinition { get; set; }
        public int? LevelId { get; set; }
        public Level Level { get; set; }
        public int? SublevelId { get; set; }
        public Sublevel Sublevel { get; set; }
        public bool AutoCorrect { get; set; }
        public int Status { get; set; }
        public double TotalPoint { get; set; }
        public DateTime? PlacementStartDate { get; set; }
        public bool IsArchived { get; set; } = false;
    }
}
