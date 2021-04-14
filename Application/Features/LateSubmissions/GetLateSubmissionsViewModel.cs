using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Features
{
    public class LateSubmissionsViewModel
    {
        public int Id { get; set; }
        // TODO: change it to view model
        public ApplicationUser Teacher { get; set; }
        public TestInstance TestInstance { get; set; } //(Object which includes Test, Student, GroupInstance)
        public DateTime? ExpectedDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public double DelayDuration { get; set; }
        public LessonInstance? LessonInstance { get; set; }
        public HomeWorkSubmition? homeworkSubmission { get; set; }
    }
}
