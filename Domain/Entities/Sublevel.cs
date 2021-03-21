using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Sublevel : AuditableBaseEntity
    {
        public Sublevel()
        {
            this.LessonDefinitions = new List<LessonDefinition>();
        }
        public string Name { get; set; }
        public int LevelId { get; set; }
        public virtual Level Level { get; set; }
        public int NumberOflessons { get; set; }
        public string Color { get; set; }
        public bool IsFinal { get; set; }
        public List<LessonDefinition> LessonDefinitions { get; set; }
        public int Quizpercent { get; set; }
        public int SublevelTestpercent { get; set; }
        public int FinalTestpercent { get; set; }
        public int Order { get; set; }
    }
}
