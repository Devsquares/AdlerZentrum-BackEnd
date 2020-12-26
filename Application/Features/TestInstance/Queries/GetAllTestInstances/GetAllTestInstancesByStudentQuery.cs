using Application.Features.TestInstance.Queries.GetAllTestInstances;
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

namespace Application.Features
{
    public class GetAllTestInstancesByStudentQuery : IRequest<IEnumerable<GetAllTestInstancesViewModel>>
    {
        public string StudentId { get; set; }
        public int GroupInstanceId { get; set; }
    }
    public class GetAllTestInstancesByStudentQueryHandler : IRequestHandler<GetAllTestInstancesByStudentQuery, IEnumerable<GetAllTestInstancesViewModel>>
    {
        private readonly ITestInstanceRepositoryAsync _testInstanceRepository;
        private readonly IMapper _mapper;
        public GetAllTestInstancesByStudentQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
        {
            _testInstanceRepository = testinstanceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllTestInstancesViewModel>> Handle(GetAllTestInstancesByStudentQuery request, CancellationToken cancellationToken)
        {
            var testinstance = await _testInstanceRepository.GetAllQuizzForStudentAsync(request.StudentId, request.GroupInstanceId);

            var testinstanceViewModel = _mapper.Map<IEnumerable<GetAllTestInstancesViewModel>>(testinstance);
            return testinstanceViewModel;
        }
    }
}
