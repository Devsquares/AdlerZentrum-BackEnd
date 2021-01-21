using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetAllTestInstancesResultsQuery : IRequest<PagedResponse<IEnumerable<TestInstancesResultsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int GroupInstanceId { get; set; }

        public class GetAllTestInstancesResultsQueryHandler : IRequestHandler<GetAllTestInstancesResultsQuery, PagedResponse<IEnumerable<TestInstancesResultsViewModel>>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            private readonly IMapper _mapper;
            public GetAllTestInstancesResultsQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
            {
                _testinstanceRepository = testinstanceRepository;
                _mapper = mapper;
            }

            public async Task<PagedResponse<IEnumerable<TestInstancesResultsViewModel>>> Handle(GetAllTestInstancesResultsQuery request, CancellationToken cancellationToken)
            {
                int count = _testinstanceRepository.GetAllTestInstancesResultsCount(request.GroupInstanceId);
                var testinstance = await _testinstanceRepository.GetAllTestInstancesResults(request.GroupInstanceId);
                var testinstanceViewModel = _mapper.Map<IEnumerable<TestInstancesResultsViewModel>>(testinstance);
                return new PagedResponse<IEnumerable<TestInstancesResultsViewModel>>(testinstanceViewModel, request.PageNumber, count);
            }
        }
    }
}
