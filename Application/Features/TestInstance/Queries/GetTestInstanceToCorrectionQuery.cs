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
    public class GetTestInstanceByTeacherQuery : IRequest<Response<IReadOnlyList<TestInstanceToAssginViewModel>>>
    {
        public string TeacherId { get; set; }
        public int? Status { get; set; }
        public int? TestType { get; set; }
    }
    public class GetTestInstanceByTeacherQueryHandler : IRequestHandler<GetTestInstanceByTeacherQuery, Response<IReadOnlyList<TestInstanceToAssginViewModel>>>
    {
        private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
        private readonly IMapper _mapper;
        public GetTestInstanceByTeacherQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
        {
            _testinstanceRepository = testinstanceRepository;
            _mapper = mapper;
        }
        public async Task<Response<IReadOnlyList<TestInstanceToAssginViewModel>>> Handle(GetTestInstanceByTeacherQuery query, CancellationToken cancellationToken)
        {
            var testinstance = await _testinstanceRepository.GetTestInstanceByTeacher(query.TeacherId, query.Status, query.TestType);
            if (testinstance == null) throw new ApiException($"TestInstance Not Found.");

            var testinstanceViewModel = _mapper.Map<IReadOnlyList<TestInstanceToAssginViewModel>>(testinstance);
            return new Response<IReadOnlyList<TestInstanceToAssginViewModel>>(testinstanceViewModel);
        }
    }

}
