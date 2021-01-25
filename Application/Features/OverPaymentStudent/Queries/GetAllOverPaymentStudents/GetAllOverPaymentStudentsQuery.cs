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

namespace Application.Features.OverPaymentStudent.Queries.GetAllOverPaymentStudents
{
    public class GetAllOverPaymentStudentsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllOverPaymentStudentsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Dictionary<string, string> FilterValue { get; set; }
        public Dictionary<string, string> FilterRange { get; set; }
        public Dictionary<string, List<string>> FilterArray { get; set; }
        public Dictionary<string, string> FilterSearch { get; set; }
        public string SortBy { get; set; }
        public string SortType { get; set; }
        public bool NoPaging { get; set; }
    }
    public class GetAllOverPaymentStudentsQueryHandler : IRequestHandler<GetAllOverPaymentStudentsQuery, FilteredPagedResponse<IEnumerable<GetAllOverPaymentStudentsViewModel>>>
    {
        private readonly IOverPaymentStudentRepositoryAsync _overpaymentstudentRepository;
        private readonly IMapper _mapper;
        public GetAllOverPaymentStudentsQueryHandler(IOverPaymentStudentRepositoryAsync overpaymentstudentRepository, IMapper mapper)
        {
            _overpaymentstudentRepository = overpaymentstudentRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllOverPaymentStudentsViewModel>>> Handle(GetAllOverPaymentStudentsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllOverPaymentStudentsParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _overpaymentstudentRepository.GetCount(validFilter);

            var overpaymentstudent = await _overpaymentstudentRepository.GetPagedReponseAsync(validFilter);
            var overpaymentstudentViewModel = _mapper.Map<IEnumerable<GetAllOverPaymentStudentsViewModel>>(overpaymentstudent);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllOverPaymentStudentsViewModel>>(overpaymentstudentViewModel, validFilter, count);
        }
    }
}
