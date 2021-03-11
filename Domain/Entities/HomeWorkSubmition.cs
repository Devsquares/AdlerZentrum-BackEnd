using Domain.Common;
using System;
using System.Data;

namespace Domain.Entities
{
    public class HomeWorkSubmition : AuditableBaseEntity
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public string URL { get; set; }
        public int HomeworkId { get; set; }
        public Homework Homework { get; set; }
        public DateTime? DueDate { get; set; }
        public string Text { get; set; }
        public string Solution { get; set; }
        public DateTime? CorrectionDate { get; set; }
        public string CorrectionTeacherId { get; set; }
        public ApplicationUser CorrectionTeacher { get; set; }
        public int Status { get; set; }
        public DateTime? SubmitionDate { get; set; }
        public int Points { get; set; }
        public string Comment { get; set; }

        public int BonusPoints { get; set; }
    }
}
