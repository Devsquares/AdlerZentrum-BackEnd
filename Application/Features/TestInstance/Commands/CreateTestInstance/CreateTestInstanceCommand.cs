using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TestInstance.Commands.CreateTestInstance
{
    public partial class CreateTestInstanceCommand : IRequest<Response<int>>
    {
        public int LessonInstanceId { get; set; }
        public int StudentId { get; set; }
        public int Points { get; set; }
        public int Status { get; set; }
        public LessonInstance LessonInstance { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class CreateTestInstanceCommandHandler : IRequestHandler<CreateTestInstanceCommand, Response<int>>
    {
        private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
        private readonly IMapper _mapper;
        public CreateTestInstanceCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository, IMapper mapper)
        {
            _testinstanceRepository = testinstanceRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateTestInstanceCommand request, CancellationToken cancellationToken)
        {
            var testinstance = _mapper.Map<Domain.Entities.TestInstance>(request);
            await _testinstanceRepository.AddAsync(testinstance);
            return new Response<int>(testinstance.Id);
        }
    }
}
