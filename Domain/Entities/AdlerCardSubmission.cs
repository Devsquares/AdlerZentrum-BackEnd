using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AdlerCardSubmission : AuditableBaseEntity
    {
        public AdlerCard AdlerCard { get; set; }
        public int AdlerCardId { get; set; }
        public ApplicationUser Student { get; set; }
        public string StudentId { get; set; }
        public ApplicationUser Teacher { get; set; }
        public string TeacherId { get; set; }
        public int Status { get; set; }
        public double AchievedScore { get; set; }
    }
}
