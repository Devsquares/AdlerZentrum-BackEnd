using Domain.Common;
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
        public string Text { get; set; }
    }
}
