using Application.DTOs.Account;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public class GetAllTeacerGroupInstanceAssignmentViewModel
    {
        public string TeacherId { get; set; }
        public virtual AccountViewModel Teacher { get; set; }
        public int GroupInstanceId { get; set; }
        public virtual GroupInstance GroupInstance { get; set; }
        public bool IsDefault { get; set; }
        public int? LessonInstanceId { get; set; }
        public virtual LessonInstance LessonInstance { get; set; }
    }
}
