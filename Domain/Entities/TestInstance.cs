using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TestInstance : AuditableBaseEntity
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public int Points { get; set; }
        public int Status { get; set; }
        public int LessonInstanceId { get; set; }
        public LessonInstance LessonInstance { get; set; }
        public DateTime StartDate { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
    }
}
