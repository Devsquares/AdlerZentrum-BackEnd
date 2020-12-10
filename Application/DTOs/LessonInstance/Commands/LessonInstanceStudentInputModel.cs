using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class LessonInstanceStudentInputModel
    {
        public int Id { get; set; }
        public int LessonInstanceId { get; set; }
        public string StudentId { get; set; }
        public bool Attend { get; set; }
        public bool Homework { get; set; }
    }
}
