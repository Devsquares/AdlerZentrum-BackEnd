using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetFinancialAnalysisReportQuery : IRequest<PagedResponse<IEnumerable<GetFinancialAnalysisReportViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string StudentName { get; set; }
        public int? GroupInstanceId { get; set; }
        public int? Category { get; set; }
    }
    public class GetFinancialAnalysisReportQueryHandler : IRequestHandler<GetFinancialAnalysisReportQuery, PagedResponse<IEnumerable<GetFinancialAnalysisReportViewModel>>>
    {
        private readonly IPaymentTransactionsRepositoryAsync _paymentTransactionsRepository;
        private readonly IMapper _mapper;
        public GetFinancialAnalysisReportQueryHandler(IPaymentTransactionsRepositoryAsync paymentTransactionsRepositoryAsync, IMapper mapper)
        {
            _paymentTransactionsRepository = paymentTransactionsRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetFinancialAnalysisReportViewModel>>> Handle(GetFinancialAnalysisReportQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetFinancialAnalysisReportParameter>(request);
            RequestParameter filteredRequestParameter = new RequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = 0;

            var singlequestionsubmission = _paymentTransactionsRepository.GetFinancialAnalysisReport(request.From, request.To, request.StudentName, request.GroupInstanceId, request.Category, validFilter.PageNumber, validFilter.PageSize, out count);

            var singlequestionsubmissionViewModel = _mapper.Map<IEnumerable<GetFinancialAnalysisReportViewModel>>(singlequestionsubmission);
            return new Wrappers.PagedResponse<IEnumerable<GetFinancialAnalysisReportViewModel>>(singlequestionsubmissionViewModel, validFilter.PageNumber, validFilter.PageSize, count);
        }
    }
}
