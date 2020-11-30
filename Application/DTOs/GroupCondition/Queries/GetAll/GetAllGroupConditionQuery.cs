using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.GroupCondition.Queries
{
    public class GetAllGroupConditionsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllGroupConditionViewModel>>>
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
    public class GetAllGroupConditionsQueryHandler : IRequestHandler<GetAllGroupConditionsQuery, FilteredPagedResponse<IEnumerable<GetAllGroupConditionViewModel>>>
    {
        private readonly IGroupConditionRepositoryAsync _GroupConditionRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllGroupConditionsQueryHandler(IGroupConditionRepositoryAsync GroupConditionService, IMapper mapper)
        {
            _GroupConditionRepositoryAsync = GroupConditionService;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllGroupConditionViewModel>>> Handle(GetAllGroupConditionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<FilteredRequestParameter>(request);
            IReadOnlyList<Domain.Entities.GroupCondition> GroupConditions;
            GroupConditions = await _GroupConditionRepositoryAsync.GetPagedReponseAsync(validFilter);
            var userViewModel = _mapper.Map<IEnumerable<GetAllGroupConditionViewModel>>(GroupConditions);
            return new FilteredPagedResponse<IEnumerable<GetAllGroupConditionViewModel>>(userViewModel, validFilter, userViewModel.ToList().Count);
        }
    }
}
