using Domain.Common;
using System;

namespace Domain.Entities
{
    public class PaymentTransaction : AuditableBaseEntity
    {
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Method { get; set; }
        public string TransactionId { get; set; }
        public int Category { get; set; }
        public int? GroupDefinitionId { get; set; }
        public GroupDefinition GroupDefinition { get; set; }
        public DateTime PurchasingDate { get; set; }
    }
}
