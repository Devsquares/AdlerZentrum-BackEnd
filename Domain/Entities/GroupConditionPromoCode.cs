using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class GroupConditionPromoCode: AuditableBaseEntity
    {
	public int GroupConditionDetailsId { get; set; }
	public virtual GroupConditionDetail GroupConditionDetails { get; set; }
	public int PromoCodeId { get; set; }
	public virtual PromoCode PromoCode { get; set; }
	public int Count { get; set; }
    }
}
