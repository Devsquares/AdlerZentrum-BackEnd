using Domain.Common;

namespace Domain.Entities
{
    public class Sublevel : AuditableBaseEntity
    {
        public string Name { get; set; }
        public int LevelId { get; set; }
        public int NumberOflessons { get; set; }
        public string Color { get; set; }
    }
}
