
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
        }
        public int GroupInstanceId { get; set; }
        public virtual GroupInstance GroupInstance { get; set; }
        public int LessonDefinitionId { get; set; }
        public virtual LessonDefinition LessonDefinition { get; set; }
        public string MaterialDone { get; set; }
        public string MaterialToDo { get; set; }
        public string Serial { get; set; }
        public bool SubmittedReport { get; set; }
        public ApplicationUser SubmittedReportTeacher { get; set; }
        public string SubmittedReportTeacherId { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public virtual IList<LessonInstanceStudent> LessonInstanceStudents { get; set; }
        public int? TestId { get; set; }
        public Test Test { get; set; }
        public int? TestStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
