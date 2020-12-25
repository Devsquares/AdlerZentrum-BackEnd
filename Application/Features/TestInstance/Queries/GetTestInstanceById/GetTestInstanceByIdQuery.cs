using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TestInstance.Queries.GetTestInstanceById
{
    public class GetTestInstanceByIdQuery : IRequest<Response<Domain.Entities.TestInstance>>
    {
        public int Id { get; set; }
        public class GetTestInstanceByIdQueryHandler : IRequestHandler<GetTestInstanceByIdQuery, Response<Domain.Entities.TestInstance>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public GetTestInstanceByIdQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<Domain.Entities.TestInstance>> Handle(GetTestInstanceByIdQuery query, CancellationToken cancellationToken)
            {
                var testinstance = await _testinstanceRepository.GetByIdAsync(query.Id);
                if (testinstance == null) throw new ApiException($"TestInstance Not Found.");
                return new Response<Domain.Entities.TestInstance>(testinstance);
            }
        }
    }
}
