using System.Collections.Generic;
using Domain.Entities;

namespace Application.DTOs
{
    public class GetAllSingleQuestionsViewModel
    {
        public int Id { get; set; }
        public int SingleQuestionType { get; set; }
        public string Text { get; set; }
        public ICollection<Choice> Choices { get; set; }
        public bool TrueOrFalse {get;set;}
    }
}