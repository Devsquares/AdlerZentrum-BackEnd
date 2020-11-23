using System.Collections;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Level : AuditableBaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Sublevel> SubLevels { get; set; }
    }
}
