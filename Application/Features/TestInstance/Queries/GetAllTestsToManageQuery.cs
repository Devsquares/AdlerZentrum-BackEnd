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
    public class GetAllTestsToManageQuery : IRequest<Response<IReadOnlyList<AllTestsToManageViewModel>>>
    {
    }
    public class GetAllTestsToManageQueryHandler : IRequestHandler<GetAllTestsToManageQuery, Response<IReadOnlyList<AllTestsToManageViewModel>>>
    {
        private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
        private readonly IMapper _mapper;
        public GetAllTestsToManageQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
        {
            _testinstanceRepository = testinstanceRepository;
            _mapper = mapper;
        }
        public async Task<Response<IReadOnlyList<AllTestsToManageViewModel>>> Handle(GetAllTestsToManageQuery query, CancellationToken cancellationToken)
        {
            var testinstance = await _testinstanceRepository.GetAllTestsToManage();
            if (testinstance == null) throw new ApiException($"TestInstance Not Found.");

            var testinstanceViewModel = _mapper.Map<IReadOnlyList<AllTestsToManageViewModel>>(testinstance);
            return new Response<IReadOnlyList<AllTestsToManageViewModel>>(testinstanceViewModel);
        }
    }
}
