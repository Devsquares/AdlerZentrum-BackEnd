using Application.DTOs.Account;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class GetFinancialAnalysisReportViewModel
    {
        public int Id { get; set; }
        public virtual AccountViewModel User { get; set; }
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
