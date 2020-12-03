using Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class LessonsByGroupInstanceIdRequestParameter : RequestParameter
    {
        public int GroupInstanceId { get; set; }
    }
}
