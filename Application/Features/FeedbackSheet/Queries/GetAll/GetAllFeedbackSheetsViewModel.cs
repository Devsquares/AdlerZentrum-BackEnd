using Application.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class GetAllFeedbackSheetsViewModel
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string GroupSerial { get; set; }
        public string LessonSerial { get; set; }
        public int Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
