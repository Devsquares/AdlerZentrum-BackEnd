using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SingleQuestion : AuditableBaseEntity
    {
        public int SingleQuestionTypeId { get; set; }
        public virtual SingleQuestionType SingleQuestionType { get; set; }
        public string Text { get; set; }
    }
}
