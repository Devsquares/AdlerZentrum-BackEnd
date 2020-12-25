using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TestInstance : AuditableBaseEntity
    {
        public int LessonInstanceId { get; set; }
        public int StudentId { get; set; }
        public int Points { get; set; }
        public int Status { get; set; }
        public LessonInstance LessonInstance { get; set; }
        public DateTime StartDate { get; set; }
    }
}
