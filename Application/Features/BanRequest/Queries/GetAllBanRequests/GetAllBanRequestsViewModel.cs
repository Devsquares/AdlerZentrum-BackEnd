using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features 
{
	public class GetAllBanRequestsViewModel
	{
		public int Id { get; set; }
		public ApplicationUser Student { get; set; }
		public string Comment { get; set; }
	}
}
