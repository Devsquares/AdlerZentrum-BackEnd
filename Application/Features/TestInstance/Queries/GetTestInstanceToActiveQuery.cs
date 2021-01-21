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
    public class GetTestInstanceToActiveQuery : IRequest<Response<IReadOnlyList<TestInstanceToAssginViewModel>>>
    {
    }
    public class GetTestInstanceToActiveQueryHandler : IRequestHandler<GetTestInstanceToActiveQuery, Response<IReadOnlyList<TestInstanceToAssginViewModel>>>
    {
        private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
        private readonly IMapper _mapper;
        public GetTestInstanceToActiveQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
        {
            _testinstanceRepository = testinstanceRepository;
            _mapper = mapper;
        }
        public async Task<Response<IReadOnlyList<TestInstanceToAssginViewModel>>> Handle(GetTestInstanceToActiveQuery query, CancellationToken cancellationToken)
        {
            var testinstance = await _testinstanceRepository.GetTestInstanceToActive();
            if (testinstance == null) throw new ApiException($"TestInstance Not Found.");

            var testinstanceViewModel = _mapper.Map<IReadOnlyList<TestInstanceToAssginViewModel>>(testinstance);
            return new Response<IReadOnlyList<TestInstanceToAssginViewModel>>(testinstanceViewModel);
        }
    }
}
