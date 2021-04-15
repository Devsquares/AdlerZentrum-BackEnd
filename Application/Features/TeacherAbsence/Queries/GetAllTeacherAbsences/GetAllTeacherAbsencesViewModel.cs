using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.TeacherAbsence.Queries.GetAllTeacherAbsences
{
	public class GetAllTeacherAbsencesViewModel
	{
		public int Id { get; set; }
		public string TeacherId { get; set; }
		public ApplicationUser Teacher { get; set; }
		public int LessonInstanceId { get; set; }
		public LessonInstance LessonInstance { get; set; }
		public bool IsEmergency { get; set; }
		public int Status { get; set; }
	}
}
