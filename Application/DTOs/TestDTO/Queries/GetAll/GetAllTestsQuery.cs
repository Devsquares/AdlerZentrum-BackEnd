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
    public class GetAllTestsQuery : IRequest<PagedResponse<IEnumerable<TestsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? TestType { get; set; }
        public int? LevelId { get; set; }
        public int? SubLevel { get; set; }
        public int? Status { get; set; }
    }
    public class GetAllTestsQueryHandler : IRequestHandler<GetAllTestsQuery, PagedResponse<IEnumerable<TestsViewModel>>>
    {
        private readonly ITestRepositoryAsync _TestService;
        private readonly IMapper _mapper;
        public GetAllTestsQueryHandler(ITestRepositoryAsync TestService, IMapper mapper)
        {
            _TestService = TestService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<TestsViewModel>>> Handle(GetAllTestsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            var testModel = await _TestService.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize, request.TestType, request.LevelId, request.SubLevel, request.Status);
            int count = _TestService.GetCount(request.TestType, request.LevelId, request.SubLevel, request.Status);
            // var userViewModel = _mapper.Map<IEnumerable<GetAllTestsViewModel>>(user);
            return new PagedResponse<IEnumerable<TestsViewModel>>(testModel, validFilter.PageNumber, validFilter.PageSize, count);
        }
    }
}
