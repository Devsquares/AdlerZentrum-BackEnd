using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.PromoCodeInstance.Queries.GetAllPromoCodeInstances
{
	public class GetAllPromoCodeInstancesViewModel
	{
		public int Id { get; set; }
		public int PromoCodeId { get; set; }
	///	public PromoCode PromoCode { get; set; }
		public string PromoCodeKey { get; set; }
		public bool IsUsed { get; set; }
		public string StudentId { get; set; }
		public string StudentEmail { get; set; }
		//public ApplicationUser Student { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}
}
