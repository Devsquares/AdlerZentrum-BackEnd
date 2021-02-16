using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class GroupInstanceModel
    {
        public int GroupDefinitionId { get; set; }
        public DateTime GroupDefinitionStartDate { get; set; }
        public DateTime GroupDefinitionEndDate { get; set; }
        public DateTime? GroupDefinitionFinalTestDate { get; set; }
        public int GroupInstanceId { get; set; }
        public string Serial { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsCurrent { get; set; } = false;


    }
}
