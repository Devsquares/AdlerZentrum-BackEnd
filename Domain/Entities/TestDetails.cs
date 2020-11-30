
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TestDetails : AuditableBaseEntity
    {
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public int Order { get; set; }
    }
}
