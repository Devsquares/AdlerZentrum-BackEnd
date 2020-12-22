using Application.DTOs.Level.Queries;
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

namespace Application.DTOs 
{
    public class GetAllTestsQuery : IRequest<PagedResponse<IEnumerable<GetAllTestsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllTestsQueryHandler : IRequestHandler<GetAllTestsQuery, PagedResponse<IEnumerable<GetAllTestsViewModel>>>
    {
        private readonly ITestRepositoryAsync _TestService;
        private readonly IMapper _mapper;
        public GetAllTestsQueryHandler(ITestRepositoryAsync TestService, IMapper mapper)
        {
            _TestService = TestService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllTestsViewModel>>> Handle(GetAllTestsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            var user = await _TestService.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var userViewModel = _mapper.Map<IEnumerable<GetAllTestsViewModel>>(user);
            return new PagedResponse<IEnumerable<GetAllTestsViewModel>>(userViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
