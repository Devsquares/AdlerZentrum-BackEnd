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
        public ApplicationUser Student { get; set; }
        public double Points { get; set; }
        public int Status { get; set; }
        public int? LessonInstanceId { get; set; }
        public virtual LessonInstance LessonInstance { get; set; }
        public string CorrectionTeacherId { get; set; }
        public ApplicationUser CorrectionTeacher { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        public bool ManualCorrection { get; set; }
        public int? GroupInstanceId { get; set; }
        public GroupInstance GroupInstance { get; set; }
        public bool ReCorrectionRequest { get; set; }
        public bool Reopened { get; set; }
        public DateTime CorrectionDueDate { get; set; }
        public DateTime CorrectionDate { get; set; }
        public bool DelaySeen { get; set; }
    }
}
