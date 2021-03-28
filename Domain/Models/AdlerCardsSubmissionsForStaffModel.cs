using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class AdlerCardsSubmissionsForStaffModel
    {
        public int AdlerCardsSubmissionsId { get; set; }
        public StudentsModel Student { get; set; }
        public Level Level { get; set; }
        public string Type { get; set; }
        public AdlerCardsUnit Unit { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedTeacherName { get; set; }
        public double Score { get; set; }
        public string status { get; set; }
    }
}
