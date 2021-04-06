using Application.Enums;
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
    public class GetTestInstanceByIdQuery : IRequest<Response<TestInstanceViewModel>>
    {
        public int Id { get; set; }
        public class GetTestInstanceByIdQueryHandler : IRequestHandler<GetTestInstanceByIdQuery, Response<TestInstanceViewModel>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            private readonly IMapper _mapper;
            public GetTestInstanceByIdQueryHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
            {
                _testinstanceRepository = testinstanceRepository;
                _mapper = mapper;
            }
            public async Task<Response<TestInstanceViewModel>> Handle(GetTestInstanceByIdQuery query, CancellationToken cancellationToken)
            {
                var testinstance = await _testinstanceRepository.GetByIdAsync(query.Id);
                if (testinstance == null) return new Response<TestInstanceViewModel>($"TestInstance Not Found.");
                var viewModel = _mapper.Map<TestInstanceViewModel>(testinstance);
                if (testinstance.Status == (int)TestInstanceEnum.Pending)
                {
                    viewModel.Timer = testinstance.Test.TestDuration - (DateTime.Now - testinstance.OpenDate.Value).TotalMinutes;
                }
                return new Response<TestInstanceViewModel>(viewModel);
            }
        }
    }
}
