using Domain.Entities;

namespace Application.DTOs.HomeWorkSubmitionDTO.Queries
{
    public class GetAllHomeWorkSubmitionsViewModel
    {
        public string URL { get; set; }
        public int HomeworkId { get; set; }
        public Homework Homework { get; set; }
        public string Text { get; set; }
        public int Status { get; set; }
    }
}
