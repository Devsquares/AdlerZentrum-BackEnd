using Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class GetFinancialAnalysisReportParameter : RequestParameter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string StudentName { get; set; }
        public int? GroupInstanceId { get; set; }
        public int? Category { get; set; }
    }
}
