using Domain.Common;
using System;

namespace Domain.Entities
{
    public class MailJob : AuditableBaseEntity
    {
        public int? TestInstanceId { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string Failure { get; set; }
        public string StudentId { get; set; }
        public string To { get; set; }
        public int? GroupInstanceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExecutionDate { get; set; }
        public DateTime? FinishDate { get; set; }

        public int? HomeworkId { get; set; }

    }
}
