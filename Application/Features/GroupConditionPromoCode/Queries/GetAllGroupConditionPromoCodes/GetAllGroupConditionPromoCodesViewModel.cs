using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.GroupConditionPromoCode.Queries.GetAllGroupConditionPromoCodes
{
	public class GetAllGroupConditionPromoCodesViewModel
	{
		public int Id { get; set; }
		public int GroupConditionDetailsId { get; set; }
		public int PromoCodeId { get; set; }
		public int Count { get; set; }
	}
}
