using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class TestInstanceViewModel
    {
        public string StudentId { get; set; }
        public double Points { get; set; }
        public int Status { get; set; }
        public int? LessonInstanceId { get; set; }
        public virtual LessonInstance LessonInstance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        public double Timer { get; set; }
    }
}
