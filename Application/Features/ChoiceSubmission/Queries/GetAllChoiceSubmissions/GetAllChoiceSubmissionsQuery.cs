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
    public class GetAllChoiceSubmissionsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllChoiceSubmissionsViewModel>>>
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
    public class GetAllChoiceSubmissionsQueryHandler : IRequestHandler<GetAllChoiceSubmissionsQuery, FilteredPagedResponse<IEnumerable<GetAllChoiceSubmissionsViewModel>>>
    {
        private readonly IChoiceSubmissionRepositoryAsync _choicesubmissionRepository;
        private readonly IMapper _mapper;
        public GetAllChoiceSubmissionsQueryHandler(IChoiceSubmissionRepositoryAsync choicesubmissionRepository, IMapper mapper)
        {
            _choicesubmissionRepository = choicesubmissionRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllChoiceSubmissionsViewModel>>> Handle(GetAllChoiceSubmissionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllChoiceSubmissionsParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _choicesubmissionRepository.GetCount(validFilter);

            var choicesubmission = await _choicesubmissionRepository.GetPagedReponseAsync(validFilter);
            var choicesubmissionViewModel = _mapper.Map<IEnumerable<GetAllChoiceSubmissionsViewModel>>(choicesubmission);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllChoiceSubmissionsViewModel>>(choicesubmissionViewModel, validFilter, count);
        }
    }
}
