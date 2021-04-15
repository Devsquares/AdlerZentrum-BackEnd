using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TeacherAbsence: AuditableBaseEntity
    {
	public string TeacherId { get; set; }
	public ApplicationUser Teacher { get; set; }
	public int LessonInstanceId { get; set; }
	public LessonInstance LessonInstance { get; set; }
	public bool IsEmergency { get; set; }
	public int Status { get; set; }
    }
}
