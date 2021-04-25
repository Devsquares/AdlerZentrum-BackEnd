using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.AccountDTO
{
    public class PaymentTransactionInputModel
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Method { get; set; }
        public string TransactionId { get; set; }
        public int Category { get; set; }
        public int? GroupDefinitionId { get; set; } 
        public DateTime PurchasingDate { get; set; }
    }
}
