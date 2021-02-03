using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class GroupInstanceStudentsViewModel
    {
        public string StudentId { get; set; }
        public int GroupInstanceId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsPlacementTest { get; set; }
        public int? PromoCodeId { get; set; }

    }
}
