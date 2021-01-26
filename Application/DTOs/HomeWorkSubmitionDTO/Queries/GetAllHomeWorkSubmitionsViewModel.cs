using Domain.Entities;
using System;

namespace Application.DTOs
{
    public class GetAllHomeWorkSubmitionsViewModel
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string Student { get; set; }
        public string URL { get; set; }
        public int HomeworkId { get; set; }
        public int GroupInstanceId { get; set; }
        public string GroupInstance { get; set; }
        public int LessonInstanceId { get; set; }
        public string LessonInstance { get; set; }
        public DateTime? DueDate { get; set; }
        public string Text { get; set; }
        public string Solution { get; set; } 
        public string Status { get; set; }
        public DateTime SubmitionDate { get; set; }
        public int Points { get; set; }
    }
}
