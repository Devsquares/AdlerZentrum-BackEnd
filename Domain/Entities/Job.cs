using Domain.Common;
using System;

namespace Domain.Entities
{
    public class Job : AuditableBaseEntity
    {
        public int? TestInstanceId { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string Failure { get; set; }
        public string StudentId { get; set; }
        public int? GroupInstanceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExecutionDate { get; set; }
        public DateTime? FinishDate { get; set; }

    }
}
