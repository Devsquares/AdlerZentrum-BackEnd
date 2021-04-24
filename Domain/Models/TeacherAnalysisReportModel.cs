using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class TeacherAnalysisReportModel
    {
        public string TeacherName { get; set; }
        public double HomeworksUpload { get; set; }
        public double HomeworksUploadDelay { get; set; }
        public double HomeworksCorrectionDelay { get; set; }
        public double TestsCorrectionDelay { get; set; }
        public double FeedbackScore { get; set; }
    }
}
