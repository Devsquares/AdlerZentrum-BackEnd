using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features 
{
	public class GetAllBugsViewModel
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string BugName { get; set; }
		public string Type { get; set; }
		public string Priority { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public string Status { get; set; }
		public string Notes { get; set; }
	}
}
