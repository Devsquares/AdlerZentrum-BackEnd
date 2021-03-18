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
            var testModel = await _TestService.GetPlacementPagedReponseAsync(request.PageNumber, request.PageSize);
            int count = _TestService.GetPlacementCount();
            return new PagedResponse<IEnumerable<TestsViewModel>>(testModel, request.PageNumber, request.PageSize, count);
        }
    }
}
