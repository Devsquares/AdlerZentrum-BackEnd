using Domain.Common;

namespace Domain.Entities
{
    public class Job : AuditableBaseEntity
    {
        public int TestInstanceId { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string Failure { get; set; }
    }
}
