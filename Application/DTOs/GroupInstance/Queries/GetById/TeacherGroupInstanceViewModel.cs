using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class TeacherGroupInstanceViewModel
    {
        public int GroupInstanceId { get; set; }
        public Domain.Entities.GroupInstance GroupInstance { get; set; }
        public bool IsDefault { get; set; }
    }
}
