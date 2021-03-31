using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class TestsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TestTypeId { get; set; }
        public LessonDefinition LessonDefinition { get; set; }
        public int? LevelId { get; set; }
        public string LevelName { get; set; }
        public int? SublevelId { get; set; }
        public string SubLevelName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
    }
}
