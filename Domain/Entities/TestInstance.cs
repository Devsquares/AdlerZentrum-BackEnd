using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class TestInstance : AuditableBaseEntity
    {
        [Required] 
        public string StudentId { get; set; } 
        public int Points { get; set; }
        public int Status { get; set; }
        public int LessonInstanceId { get; set; }
        public virtual LessonInstance LessonInstance { get; set; }
        public string CorrectionTeacherId { get; set; }
        public ApplicationUser CorrectionTeacher { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
    }
}
