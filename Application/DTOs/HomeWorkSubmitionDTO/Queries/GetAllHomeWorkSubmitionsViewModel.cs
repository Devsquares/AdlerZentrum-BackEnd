using Domain.Entities;
using System;

namespace Application.DTOs.HomeWorkSubmitionDTO.Queries
{
    public class GetAllHomeWorkSubmitionsViewModel
    {
        public string URL { get; set; }
        public int HomeworkId { get; set; }
        public Homework Homework { get; set; }
        public DateTime DueDate { get; set; }
        public string Text { get; set; }
        public string Solution { get; set; }
        public DateTime CorrectionDate { get; set; }
        public string CorrectionTeacherId { get; set; }
        public ApplicationUser CorrectionTeacher { get; set; }
        public string Status { get; set; }
    }
}
