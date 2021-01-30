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
    public class GetAllGroupConditionPromoCodesQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllGroupConditionPromoCodesViewModel>>>
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
    public class GetAllGroupConditionPromoCodesQueryHandler : IRequestHandler<GetAllGroupConditionPromoCodesQuery, FilteredPagedResponse<IEnumerable<GetAllGroupConditionPromoCodesViewModel>>>
    {
        private readonly IGroupConditionPromoCodeRepositoryAsync _groupconditionpromocodeRepository;
        private readonly IMapper _mapper;
        public GetAllGroupConditionPromoCodesQueryHandler(IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository, IMapper mapper)
        {
            _groupconditionpromocodeRepository = groupconditionpromocodeRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllGroupConditionPromoCodesViewModel>>> Handle(GetAllGroupConditionPromoCodesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllGroupConditionPromoCodesParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _groupconditionpromocodeRepository.GetCount(validFilter);

            var groupconditionpromocode = await _groupconditionpromocodeRepository.GetPagedReponseAsync(validFilter);
            var groupconditionpromocodeViewModel = _mapper.Map<IEnumerable<GetAllGroupConditionPromoCodesViewModel>>(groupconditionpromocode);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllGroupConditionPromoCodesViewModel>>(groupconditionpromocodeViewModel, validFilter, count);
        }
    }
}
