using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.InterestedStudent.Queries.GetAllInterestedStudents
{
	public class GetAllInterestedStudentsViewModel
	{
		public int Id { get; set; }
		public int PromoCodeId { get; set; }
		public string StudentId { get; set; }
		public int GroupDefinitionId { get; set; }
	}
}
