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

namespace Application.Features.TestInstance.Queries.GetAllTestInstances
{
    public class GetAllTestInstancesQuery : IRequest<FilteredPagedResponse<IEnumerable<GetAllTestInstancesViewModel>>>
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
    public class GetAllTestInstancesQueryHandler : IRequestHandler<GetAllTestInstancesQuery, FilteredPagedResponse<IEnumerable<GetAllTestInstancesViewModel>>>
    {
        private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
        private readonly IMapper _mapper;
        public GetAllTestInstancesQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
        {
            _testinstanceRepository = testinstanceRepository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedResponse<IEnumerable<GetAllTestInstancesViewModel>>> Handle(GetAllTestInstancesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllTestInstancesParameter>(request);
            FilteredRequestParameter filteredRequestParameter = new FilteredRequestParameter();
            Reflection.CopyProperties(validFilter, filteredRequestParameter);
            int count = _testinstanceRepository.GetCount(validFilter);

            var testinstance = await _testinstanceRepository.GetPagedReponseAsync(validFilter);
            var testinstanceViewModel = _mapper.Map<IEnumerable<GetAllTestInstancesViewModel>>(testinstance);
            return new Wrappers.FilteredPagedResponse<IEnumerable<GetAllTestInstancesViewModel>>(testinstanceViewModel, validFilter, count);
        }
    }
}
