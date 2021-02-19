using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class AdlerCardsUnit : AuditableBaseEntity
    {
        public string Name { get; set; }
        public Level Level { get; set; }
        public int LevelId { get; set; }
        public AdlerCardsType AdlerCardsType { get; set; }
        public int AdlerCardsTypeId { get; set; }
        public int Order { get; set; }

    }
}
