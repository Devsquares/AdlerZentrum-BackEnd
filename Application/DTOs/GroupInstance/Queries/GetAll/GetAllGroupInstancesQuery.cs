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
    public class GetAllGroupInstancesQuery : IRequest<PagedResponse<IEnumerable<GetAllGroupInstancesViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? groupDefinationId { get; set; }
        public List<int> Status { get; set; }
    }
    public class GetAllGroupInstancesQueryHandler : IRequestHandler<GetAllGroupInstancesQuery, PagedResponse<IEnumerable<GetAllGroupInstancesViewModel>>>
    {
        private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllGroupInstancesQueryHandler(IGroupInstanceRepositoryAsync GroupInstanceService, IMapper mapper)
        {
            _groupInstanceRepositoryAsync = GroupInstanceService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllGroupInstancesViewModel>>> Handle(GetAllGroupInstancesQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber == 0) request.PageNumber = 1;
            if (request.PageSize == 0) request.PageSize = 10;
            IReadOnlyList<Domain.Entities.GroupInstance> groupInstances;
            int count = 0;
            groupInstances = _groupInstanceRepositoryAsync.GetPagedGroupInstanceReponseAsync(request.PageNumber, request.PageSize, request.groupDefinationId, request.Status, out count);
            var userViewModel = _mapper.Map<IEnumerable<GetAllGroupInstancesViewModel>>(groupInstances);
            return new PagedResponse<IEnumerable<GetAllGroupInstancesViewModel>>(userViewModel, request.PageNumber, request.PageSize, count);
        }
    }
}
