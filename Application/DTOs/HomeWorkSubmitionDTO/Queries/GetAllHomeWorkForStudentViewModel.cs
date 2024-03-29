﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Application.DTOs.Account;

namespace Application.DTOs
{
    public class GetAllHomeWorkForStudentViewModel
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public AccountViewModel Student { get; set; }
        public string URL { get; set; }
        public int HomeworkId { get; set; }
        public Homework Homework { get; set; }
        public DateTime? DueDate { get; set; }
        public string Text { get; set; }
        public string Solution { get; set; }
        public DateTime? CorrectionDate { get; set; }
        public string CorrectionTeacherId { get; set; }
        public ApplicationUser CorrectionTeacher { get; set; }
        public string Status { get; set; }
        public DateTime? SubmitionDate { get; set; }
        public double Points { get; set; }
        public string Comment { get; set; }
        public double BonusPoints { get; set; }
    }
}
