using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class StudentsModel
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public int? PromoCodeId { get; set; }
        public string PromoCodeName { get; set; }
        public int? PromoCodeValue { get; set; }
        public bool isPlacementTest { get; set; }
        public DateTime? CreationDate { get; set; }
        public string ProfilePhoto { get; set; }
        public int? PromoCodeInstanceId { get; set; }
    }
}
