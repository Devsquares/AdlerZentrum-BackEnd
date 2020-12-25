using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.TestInstance.Queries.GetAllTestInstances
{
	public class GetAllTestInstancesViewModel
	{
		public int Id { get; set; }
		public int LessonInstanceId { get; set; }
		public int StudentId { get; set; }
		public int Points { get; set; }
		public int Status { get; set; }
		public LessonInstance LessonInstance { get; set; }
		public DateTime StartDate { get; set; }
	}
}
