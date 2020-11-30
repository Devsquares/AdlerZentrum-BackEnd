using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Homework : AuditableBaseEntity
    {
        public string Text { get; set; }
        public int MinCharacters { get; set; }
        public int Points { get; set; }
        public int BonusPoints { get; set; }
    }
}
