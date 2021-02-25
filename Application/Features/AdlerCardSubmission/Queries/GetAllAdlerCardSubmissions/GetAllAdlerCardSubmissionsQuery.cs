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
    public class GetAllAdlerCardSubmissionsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllAdlerCardSubmissionsViewModel>>>
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
    public class GetAllAdlerCardSubmissionsQueryHandler : IRequestHandler<GetAllAdlerCardSubmissionsQuery, FilteredPagedResponse<IEnumerable<GetAllAdlerCardSubmissionsViewModel>>>
    {
        private readonly IAdlerCardSubmissionRepositoryAsync _adlercardsubmissionRepository;
        private readonly IMapper _mapper;
        public GetAllAdlerCardSubmissionsQueryHandler(IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository, IMapper mapper)
        {
            _adlercardsubmissionRepository = adlercardsubmissionRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllAdlerCardSubmissionsViewModel>>> Handle(GetAllAdlerCardSubmissionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllAdlerCardSubmissionsParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _adlercardsubmissionRepository.GetCount(validFilter);

            var adlercardsubmission = await _adlercardsubmissionRepository.GetPagedReponseAsync(validFilter);
            var adlercardsubmissionViewModel = _mapper.Map<IEnumerable<GetAllAdlerCardSubmissionsViewModel>>(adlercardsubmission);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllAdlerCardSubmissionsViewModel>>(adlercardsubmissionViewModel, validFilter, count);
        }
    }
}
