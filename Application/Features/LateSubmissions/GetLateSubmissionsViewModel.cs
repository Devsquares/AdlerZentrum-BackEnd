using System;
using Domain.Entities;

namespace Application.Features
{
    public class LateSubmissionsViewModel
    {
        public int Id { get; set; }
        // TODO: change it to view model
        public string Teacher { get; set; }
        public TestInstance TestInstance { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public double DelayDuration { get; set; }
        public LessonInstance? LessonInstance { get; set; }
        public HomeWorkSubmition? homeworkSubmission { get; set; }
        public GroupInstance? GroupInstance { get; set; }
        public Homework? Homework { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public Test? Test { get; set; }
    }
}
