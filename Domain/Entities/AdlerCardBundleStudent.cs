using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AdlerCardBundleStudent: AuditableBaseEntity
    {
	public string StudentId { get; set; }
	public  ApplicationUser Student { get; set; }
	public int AdlerCardsBundleId { get; set; }
	public  AdlerCardsBundle AdlerCardsBundle { get; set; }
	public DateTime PurchasingDate { get; set; }
    }
}
