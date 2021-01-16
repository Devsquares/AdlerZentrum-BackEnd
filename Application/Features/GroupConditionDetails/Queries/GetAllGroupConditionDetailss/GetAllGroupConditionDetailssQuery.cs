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

namespace Application.Features.GroupConditionDetails.Queries.GetAllGroupConditionDetailss
{
    public class GetAllGroupConditionDetailssQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllGroupConditionDetailssViewModel>>>
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
    public class GetAllGroupConditionDetailssQueryHandler : IRequestHandler<GetAllGroupConditionDetailssQuery, FilteredPagedResponse<IEnumerable<GetAllGroupConditionDetailssViewModel>>>
    {
        private readonly IGroupConditionDetailsRepositoryAsync _groupconditiondetailsRepository;
        private readonly IMapper _mapper;
        public GetAllGroupConditionDetailssQueryHandler(IGroupConditionDetailsRepositoryAsync groupconditiondetailsRepository, IMapper mapper)
        {
            _groupconditiondetailsRepository = groupconditiondetailsRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllGroupConditionDetailssViewModel>>> Handle(GetAllGroupConditionDetailssQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllGroupConditionDetailssParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _groupconditiondetailsRepository.GetCount(validFilter);

            var groupconditiondetails = await _groupconditiondetailsRepository.GetPagedReponseAsync(validFilter);
            var groupconditiondetailsViewModel = _mapper.Map<IEnumerable<GetAllGroupConditionDetailssViewModel>>(groupconditiondetails);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllGroupConditionDetailssViewModel>>(groupconditiondetailsViewModel, validFilter, count);
        }
    }
}
