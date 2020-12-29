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
    public class GetAllSingleQuestionSubmissionsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllSingleQuestionSubmissionsViewModel>>>
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
    public class GetAllSingleQuestionSubmissionsQueryHandler : IRequestHandler<GetAllSingleQuestionSubmissionsQuery, FilteredPagedResponse<IEnumerable<GetAllSingleQuestionSubmissionsViewModel>>>
    {
        private readonly ISingleQuestionSubmissionRepositoryAsync _singlequestionsubmissionRepository;
        private readonly IMapper _mapper;
        public GetAllSingleQuestionSubmissionsQueryHandler(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository, IMapper mapper)
        {
            _singlequestionsubmissionRepository = singlequestionsubmissionRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllSingleQuestionSubmissionsViewModel>>> Handle(GetAllSingleQuestionSubmissionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllSingleQuestionSubmissionsParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _singlequestionsubmissionRepository.GetCount(validFilter);

            var singlequestionsubmission = await _singlequestionsubmissionRepository.GetPagedReponseAsync(validFilter);
            var singlequestionsubmissionViewModel = _mapper.Map<IEnumerable<GetAllSingleQuestionSubmissionsViewModel>>(singlequestionsubmission);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllSingleQuestionSubmissionsViewModel>>(singlequestionsubmissionViewModel, validFilter, count);
        }
    }
}
