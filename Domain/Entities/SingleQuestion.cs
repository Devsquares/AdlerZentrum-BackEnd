using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SingleQuestion : AuditableBaseEntity
    {
        public int? QuestionId { get; set; }
        public int SingleQuestionType { get; set; }
        public string Text { get; set; }
        public ICollection<Choice> Choices { get; set; }
        public bool AnswerIsTrueOrFalse { get; set; }
        public double Points { get; set; }
    }
}
    