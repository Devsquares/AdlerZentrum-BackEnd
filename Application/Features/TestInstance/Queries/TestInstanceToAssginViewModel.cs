﻿using Application.DTOs.Account;
using Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class TestInstanceToAssginViewModel
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string GroupSerial { get; set; }
        public string TestName { get; set; }
        public TestTypeEnum TestType { get; set; }
        public int Status { get; set; }
        public bool ManualCorrection { get; set; }
        public double Points { get; set; }
        public DateTime CorrectionDueDate { get; set; }
        public DateTime CorrectionDate { get; set; }
        public string CorrectionTeacherId { get; set; }
        public AccountViewModel CorrectionTeacher { get; set; }
    }
}
