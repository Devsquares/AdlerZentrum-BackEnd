using Application.Exceptions;
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
    public class GetAllTestsToManageQuery : IRequest<PagedResponse<IReadOnlyList<AllTestsToManageViewModel>>>
    {
        public int? GroupDefinitionId { get; set; }
        public int? GroupInstanceId { get; set; }
        public int? TestTypeId { get; set; }
        public int? Status { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllTestsToManageQueryHandler : IRequestHandler<GetAllTestsToManageQuery, PagedResponse<IReadOnlyList<AllTestsToManageViewModel>>>
    {
        private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
        private readonly IMapper _mapper;
        public GetAllTestsToManageQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
        {
            _testinstanceRepository = testinstanceRepository;
            _mapper = mapper;
        }
        public async Task<PagedResponse<IReadOnlyList<AllTestsToManageViewModel>>> Handle(GetAllTestsToManageQuery query, CancellationToken cancellationToken)
        {
            if (query.PageNumber == 0) query.PageNumber = 1;
            if (query.PageSize == 0) query.PageSize = 10;
            var testinstance = await _testinstanceRepository.GetAllTestsToManage(query.GroupDefinitionId, query.GroupInstanceId, query.TestTypeId, query.Status, query.PageNumber, query.PageSize);
            if (testinstance == null) throw new ApiException($"TestInstance Not Found.");
            int count = _testinstanceRepository.GetAllTestsToManageCount(query.GroupDefinitionId, query.GroupInstanceId, query.TestTypeId, query.Status).Result;
            var testinstanceViewModel = _mapper.Map<IReadOnlyList<AllTestsToManageViewModel>>(testinstance);
            return new PagedResponse<IReadOnlyList<AllTestsToManageViewModel>>(testinstanceViewModel, query.PageNumber, query.PageSize, count);
        }
    }
}
