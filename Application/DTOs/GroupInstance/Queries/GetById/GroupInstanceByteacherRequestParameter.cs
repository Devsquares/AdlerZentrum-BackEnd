using Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.GroupInstance.Queries.GetAll
{
    public class GroupInstanceByTeacherRequestParameter : RequestParameter
    {
        public string teacherId { get; set; }
        public List<int> Status { get; set; }
    }
}
