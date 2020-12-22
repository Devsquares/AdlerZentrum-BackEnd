using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class GetAllQuestionsViewModel
    {
        public int Id { get; set; }
        public int QuestionTypeId { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public int MinCharacters { get; set; }
        public string AudioPath { get; set; }
        public int NoOfRepeats { get; set; }
        public virtual ICollection<QuestionDetails> QuestionDetails { get; set; }

    }
}
