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
    public class GetAllBanRequestsQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllBanRequestsViewModel>>>
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
    public class GetAllBanRequestsQueryHandler : IRequestHandler<GetAllBanRequestsQuery, FilteredPagedResponse<IEnumerable<GetAllBanRequestsViewModel>>>
    {
        private readonly IBanRequestRepositoryAsync _banrequestRepository;
        private readonly IMapper _mapper;
        public GetAllBanRequestsQueryHandler(IBanRequestRepositoryAsync banrequestRepository, IMapper mapper)
        {
            _banrequestRepository = banrequestRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllBanRequestsViewModel>>> Handle(GetAllBanRequestsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllBanRequestsParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _banrequestRepository.GetCount(validFilter);

            var banrequest = await _banrequestRepository.GetPagedReponseAsync(validFilter);
            var banrequestViewModel = _mapper.Map<IEnumerable<GetAllBanRequestsViewModel>>(banrequest);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllBanRequestsViewModel>>(banrequestViewModel, validFilter, count);
        }
    }
}
