
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class LessonInstance : AuditableBaseEntity
    {
        public LessonInstance()
        {
            LessonInstanceStudents = new HashSet<LessonInstanceStudent>();
        }
        public int GroupInstanceId { get; set; }
        public virtual GroupInstance GroupInstance { get; set; }
        public int LessonDefinitionId { get; set; }
        public virtual LessonDefinition LessonDefinition { get; set; }
        public int MaterialDone { get; set; }
        public int MaterialToDo { get; set; }
        public int? HomeworkId { get; set; }
        public virtual Homework Homework { get; set; }
        public virtual ICollection<LessonInstanceStudent> LessonInstanceStudents { get; private set; }
    }
}
