
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class LessonInstanceStudent : AuditableBaseEntity
    {
        public int LessonInstanceId { get; set; }
        public virtual LessonInstance LessonInstance { get; set; }
        public int StudentId { get; set; }
        public virtual ApplicationUser Student { get; set; }
        public bool Attend { get; set; }
        public bool Homework { get; set; }
    }
}
