using Domain.Entities;
using System;

namespace Application.DTOs
{
    public class GetAllomeworkBounsViewModel
    {
        public string Text { get; set; }
        public int MinCharacters { get; set; }
        public int Points { get; set; }
        public int BonusPoints { get; set; }
        public int BonusPointsStatus { get; set; }
        public int GroupInstanceId { get; set; }
        public string GroupInstanceSerial { get; set; }
        public int LessonInstanceId { get; set; }
        public string LessonInstanceSerial { get; set; }
        public string TeacherName { get; set; }
        public string LessonOrder { get; set; }
        public string SubLevel { get; set; }
    }
}
