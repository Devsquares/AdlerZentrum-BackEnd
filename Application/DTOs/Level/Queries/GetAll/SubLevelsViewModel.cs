using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class SubLevelsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LevelId { get; set; }
        public int NumberOflessons { get; set; }
        public string Color { get; set; }
        public bool IsFinal { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<LessonDefinitionViewModel> LessonDefinitions { get; set; }
        public int Quizpercent { get; set; }
        public int SublevelTestpercent { get; set; }
        public int FinalTestpercent { get; set; }
    }
}
