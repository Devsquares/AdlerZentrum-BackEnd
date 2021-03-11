using Application.DTOs.Level.Queries;
using Application.Enums;
using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllPlacementTestsQuery : IRequest<PagedResponse<IEnumerable<TestsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllPlacementTestsQueryHandler : IRequestHandler<GetAllPlacementTestsQuery, PagedResponse<IEnumerable<TestsViewModel>>>
    {
        private readonly ITestRepositoryAsync _TestService;
        private readonly IMapper _mapper;
        public GetAllPlacementTestsQueryHandler(ITestRepositoryAsync TestService, IMapper mapper)
        {
            _TestService = TestService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<TestsViewModel>>> Handle(GetAllPlacementTestsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            var testModel = await _TestService.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize, (int)TestTypeEnum.placement, null, null, null);
            int count = _TestService.GetCount((int)TestTypeEnum.placement, null, null, null);
            return new PagedResponse<IEnumerable<TestsViewModel>>(testModel, validFilter.PageNumber, validFilter.PageSize, count);
        }
    }
}
