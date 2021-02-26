using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class CurrentProgressModel
    {
        public double AchievedScore { get; set; }
        public double TotalScore { get; set; }
        public QuizzesProgressModel Quizzes { get; set; } = new QuizzesProgressModel();
        public SubLevelTestProgressModel Sublevels { get; set; } = new SubLevelTestProgressModel();
        public FinalTestProgressModel Final { get; set; } = new FinalTestProgressModel();
    }
}
