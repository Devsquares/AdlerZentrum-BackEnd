using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.GroupInstance.Queries
{
    public class GetAllGroupInstancesQuery : IRequest<PagedResponse<IEnumerable<GetAllGroupInstancesViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
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
            var validFilter = _mapper.Map<RequestParameter>(request);
            var user = await _groupInstanceRepositoryAsync.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var userViewModel = _mapper.Map<IEnumerable<GetAllGroupInstancesViewModel>>(user);
            return new PagedResponse<IEnumerable<GetAllGroupInstancesViewModel>>(userViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
