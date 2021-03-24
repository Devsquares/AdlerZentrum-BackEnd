using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class StudentInfo : AuditableBaseEntity
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public bool SublevelSuccess { get; set; }
        public int SublevelId { get; set; }
        public Sublevel Sublevel { get; set; }
    }
}