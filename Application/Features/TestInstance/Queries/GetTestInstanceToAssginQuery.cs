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
    public class GetTestInstanceToAssginQuery : IRequest<PagedResponse<IReadOnlyList<TestInstanceToAssginViewModel>>>
    {
        public string StudentName { get; set; }
        public string TestName { get; set; }
        public int? TestType { get; set; }
        public bool Assigend { get; set; }
        public int? GroupInsatanceId { get; set; }
        public int? TestInstanceId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
    }
    public class GetTestInstanceToAssginQueryHandler : IRequestHandler<GetTestInstanceToAssginQuery, PagedResponse<IReadOnlyList<TestInstanceToAssginViewModel>>>
    {
        private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
        private readonly IMapper _mapper;
        public GetTestInstanceToAssginQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
        {
            _testinstanceRepository = testinstanceRepository;
            _mapper = mapper;
        }
        public async Task<PagedResponse<IReadOnlyList<TestInstanceToAssginViewModel>>> Handle(GetTestInstanceToAssginQuery query, CancellationToken cancellationToken)
        {
            int count = 0;
            if (query.PageNumber == 0) query.PageNumber = 1;
            if (query.PageSize == 0) query.PageSize = 10;
            var testinstance = _testinstanceRepository.GetTestInstanceToAssgin(query.StudentName, query.TestName, query.TestType, query.Assigend, query.GroupInsatanceId, query.TestInstanceId, query.PageNumber, query.PageSize, out count);

            var testinstanceViewModel = _mapper.Map<IReadOnlyList<TestInstanceToAssginViewModel>>(testinstance);
            return new PagedResponse<IReadOnlyList<TestInstanceToAssginViewModel>>(testinstanceViewModel, query.PageNumber, query.PageSize, count);
        }
    }

}
