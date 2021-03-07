using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class PromoCodeInstance: AuditableBaseEntity
    {
	public int PromoCodeId { get; set; }
	public PromoCode PromoCode { get; set; }
	public string PromoCodeKey { get; set; }
	public bool IsUsed { get; set; }
	public string StudentId { get; set; }
	public string StudentEmail { get; set; }
	public ApplicationUser Student { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
    }
}
