using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Question : AuditableBaseEntity
    {
        public int QuestionTypeId { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public int MinCharacters { get; set; }
        public string AudioPath { get; set; }
        public int NoOfRepeats { get; set; }
        public virtual ICollection<QuestionDetails> QuestionDetails { get; set; }
    }
}
