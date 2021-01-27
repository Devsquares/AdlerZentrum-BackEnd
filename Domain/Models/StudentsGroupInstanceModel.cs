using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class StudentsGroupInstanceModel
    {
        public int GroupInstanceId { get; set; }
        public string Status { get; set; }
        public List<StudentsModel> Students { get; set; } = new List<StudentsModel>();
        public string Teachername { get; set; }
    }
}
