using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AdlerCardsUnit : AuditableBaseEntity
    {
        public string Name { get; set; }
        public Level Level { get; set; }
        public int LevelId { get; set; }
        public int AdlerCardsTypeId { get; set; }
        public int Order { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public virtual List<AdlerCard> AdlerCards { get; set; }
    }
}
