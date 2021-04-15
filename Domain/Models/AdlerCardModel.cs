using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class AdlerCardModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdlerCardsUnitId { get; set; }
        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public int AllowedDuration { get; set; }
        public double TotalScore { get; set; }
        public string Status { get; set; }
        public int AdlerCardsTypeId { get; set; }
        public Level Level { get; set; }
        public int LevelId { get; set; }
        public int AdlerCardSubmissionStatus { get; set; }
        public ICollection<SingleQuestion> SingleQuestions { get; set; }
    }
}
