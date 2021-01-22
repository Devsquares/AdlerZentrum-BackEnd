using Application.Features.TestInstance.Queries.GetAllTestInstances;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetQuizzesByGroupInstanceQuery : IRequest<IEnumerable<object>>
    {
        public int GroupInstanceId { get; set; }
    }

    public class GetQuizzesByGroupInstanceQueryHandler : IRequestHandler<GetQuizzesByGroupInstanceQuery, IEnumerable<object>>
    {
        private readonly ITestInstanceRepositoryAsync _testInstanceRepository;
        private readonly IMapper _mapper;
        public GetQuizzesByGroupInstanceQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
        {
            _testInstanceRepository = testinstanceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<object>> Handle(GetQuizzesByGroupInstanceQuery request, CancellationToken cancellationToken)
        {
            var res = await _testInstanceRepository.GetAllClosedAndPendingQuizzAsync(request.GroupInstanceId);
            return res;
        }
    }
}
