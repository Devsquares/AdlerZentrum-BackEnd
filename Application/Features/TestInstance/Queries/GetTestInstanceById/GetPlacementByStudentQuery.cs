using Application.Exceptions;
using Application.Features;
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
    public class GetPlacementByStudentQuery : IRequest<Response<GetAllTestInstancesViewModel>>
    {
        public string StudentId { get; set; }
        public class GetPlacementByStudentQueryHandler : IRequestHandler<GetPlacementByStudentQuery, Response<GetAllTestInstancesViewModel>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            private readonly IMapper _mapper;
            public GetPlacementByStudentQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
            {
                _testinstanceRepository = testinstanceRepository;
                _mapper = mapper;
            }
            public async Task<Response<GetAllTestInstancesViewModel>> Handle(GetPlacementByStudentQuery query, CancellationToken cancellationToken)
            {
                var testinstance = await _testinstanceRepository.GetAllPlacementTestsByStudent(query.StudentId);
                if (testinstance == null) throw new ApiException($"Test Instance Not Found.");
                var viewModel = _mapper.Map<GetAllTestInstancesViewModel>(testinstance);
                return new Response<GetAllTestInstancesViewModel>(viewModel);
            }
        }
    }
}
