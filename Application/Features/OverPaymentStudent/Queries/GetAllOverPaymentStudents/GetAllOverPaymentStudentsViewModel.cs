using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.OverPaymentStudent.Queries.GetAllOverPaymentStudents
{
	public class GetAllOverPaymentStudentsViewModel
	{
		public int Id { get; set; }
		public string StudentId { get; set; }
		public int GroupDefinitionId { get; set; }
	}
}
