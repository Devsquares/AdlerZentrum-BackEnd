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

namespace Application.Features.AdlerCardBundleStudent.Queries.GetAllAdlerCardBundleStudents
{
    public class GetAllAdlerCardBundleStudentsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllAdlerCardBundleStudentsViewModel>>>
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
    public class GetAllAdlerCardBundleStudentsQueryHandler : IRequestHandler<GetAllAdlerCardBundleStudentsQuery, FilteredPagedResponse<IEnumerable<GetAllAdlerCardBundleStudentsViewModel>>>
    {
        private readonly IAdlerCardBundleStudentRepositoryAsync _adlercardbundlestudentRepository;
        private readonly IMapper _mapper;
        public GetAllAdlerCardBundleStudentsQueryHandler(IAdlerCardBundleStudentRepositoryAsync adlercardbundlestudentRepository, IMapper mapper)
        {
            _adlercardbundlestudentRepository = adlercardbundlestudentRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllAdlerCardBundleStudentsViewModel>>> Handle(GetAllAdlerCardBundleStudentsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllAdlerCardBundleStudentsParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _adlercardbundlestudentRepository.GetCount(validFilter);

            var adlercardbundlestudent = await _adlercardbundlestudentRepository.GetPagedReponseAsync(validFilter);
            var adlercardbundlestudentViewModel = _mapper.Map<IEnumerable<GetAllAdlerCardBundleStudentsViewModel>>(adlercardbundlestudent);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllAdlerCardBundleStudentsViewModel>>(adlercardbundlestudentViewModel, validFilter, count);
        }
    }
}
