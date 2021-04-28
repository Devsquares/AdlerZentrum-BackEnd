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

namespace Application.DTOs.GroupInstance.Queries
{
    public class GetAllGroupInstancesQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllGroupInstancesViewModel>>>
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
        public List<int> Status { get; set; }
    }
    public class GetAllGroupInstancesQueryHandler : IRequestHandler<GetAllGroupInstancesQuery, FilteredPagedResponse<IEnumerable<GetAllGroupInstancesViewModel>>>
    {
        private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllGroupInstancesQueryHandler(IGroupInstanceRepositoryAsync GroupInstanceService, IMapper mapper)
        {
            _groupInstanceRepositoryAsync = GroupInstanceService;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllGroupInstancesViewModel>>> Handle(GetAllGroupInstancesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<FilteredRequestParameter>(request);
            IReadOnlyList<Domain.Entities.GroupInstance> groupInstances;
            int count = 0;
            groupInstances = _groupInstanceRepositoryAsync.GetPagedGroupInstanceReponseAsync(validFilter, request.Status, out count);
            var userViewModel = _mapper.Map<IEnumerable<GetAllGroupInstancesViewModel>>(groupInstances);
            return new FilteredPagedResponse<IEnumerable<GetAllGroupInstancesViewModel>>(userViewModel, validFilter, count);
        }
    }
}
