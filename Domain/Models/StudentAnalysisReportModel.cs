using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class StudentAnalysisReportModel
    {
        public string StudentName { get; set; }
        public double Attendance { get; set; }
        public double LateSubmissions { get; set; }
        public double MissedSubmissions { get; set; }
        public double CurrentProgressPoints { get; set; }
    }
}
