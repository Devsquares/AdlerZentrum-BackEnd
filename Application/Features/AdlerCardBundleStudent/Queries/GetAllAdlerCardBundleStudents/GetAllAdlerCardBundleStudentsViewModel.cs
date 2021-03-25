using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features 
{
	public class GetAllAdlerCardBundleStudentsViewModel
	{
		public int Id { get; set; }
		public string StudentId { get; set; }
		public  ApplicationUser Student { get; set; }
		public int AdlerCardsBundleId { get; set; }
		public  AdlerCardsBundle AdlerCardsBundle { get; set; }
		public DateTime PurchasingDate { get; set; }
	}
}
