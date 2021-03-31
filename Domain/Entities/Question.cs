using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Question : AuditableBaseEntity
    {
        public int? TestId { get; set; }
        public int QuestionTypeId { get; set; }
        public int Order { get; set; }
        public string Header { get; set; }
        public int? MinCharacters { get; set; }
        public string AudioPath { get; set; }
        public int? NoOfRepeats { get; set; }
        public string Text { get; set; }
        public virtual ICollection<SingleQuestion> SingleQuestions { get; set; }
        public bool IsAdlerService { get; set; } = false;
        public double TotalPoint { get; set; }
    }
}
