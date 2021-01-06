using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetTestInstanceToAssginQuery : IRequest<Response<IReadOnlyList<Domain.Entities.TestInstance>>>
    { 
        public class GetTestInstanceToAssginQueryHandler : IRequestHandler<GetTestInstanceToAssginQuery, Response<IReadOnlyList<Domain.Entities.TestInstance>>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public GetTestInstanceToAssginQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<IReadOnlyList<Domain.Entities.TestInstance>>> Handle(GetTestInstanceToAssginQuery query, CancellationToken cancellationToken)
            {
                var testinstance = await _testinstanceRepository.GetTestInstanceToAssgin();
                if (testinstance == null) throw new ApiException($"TestInstance Not Found.");
                return new Response<IReadOnlyList< Domain.Entities.TestInstance>>(testinstance);
            }
        }
    }
}
