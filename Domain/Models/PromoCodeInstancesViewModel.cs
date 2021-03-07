using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class PromoCodeInstancesViewModel
    {
        public int Id { get; set; }
        public int PromoCodeId { get; set; }
        public string PromoCodeName { get; set; }
        public string PromoCodeKey { get; set; }
        public bool IsUsed { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsValid { get; set; }

    }
}
