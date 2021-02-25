using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AdlerCardsBundle : AuditableBaseEntity
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTill { get; set; }
        public int Status { get; set; }
    }
}
