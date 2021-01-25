using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features 
{
	public class GetAllDisqualificationRequestsViewModel
	{
		public int Id { get; set; }
		public string StudentId { get; set; }
		public ApplicationUser Student { get; set; }
		public string Comment { get; set; }
		public int DisqualificationRequestStatus { get; set; }
	}
}
