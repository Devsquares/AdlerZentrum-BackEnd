﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class SubLevelTestProgressModel
    {
        public double AchievedScore { get; set; }
        public double TotalScore { get; set; }
        public List<TestInstance> SubLevelTests { get; set; }
    }
}
