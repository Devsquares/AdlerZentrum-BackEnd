using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetHomeWorkSubmitionByIdQuery : IRequest<GetAllHomeWorkSubmitionsViewModel>
    {
        public int HomeWorkSubmitionId { get; set; }
    }
    public class GetHomeWorkSubmitionByIdQueryHandler : IRequestHandler<GetHomeWorkSubmitionByIdQuery, GetAllHomeWorkSubmitionsViewModel>
    {
        private readonly IHomeWorkSubmitionRepositoryAsync _HomeWorkSubmitionRepository;
        private readonly IMapper _mapper;
        public GetHomeWorkSubmitionByIdQueryHandler(IHomeWorkSubmitionRepositoryAsync HomeWorkSubmitionRepository, IMapper mapper)
        {
            _HomeWorkSubmitionRepository = HomeWorkSubmitionRepository;
            _mapper = mapper;
        }

        public async Task<GetAllHomeWorkSubmitionsViewModel> Handle(GetHomeWorkSubmitionByIdQuery request, CancellationToken cancellationToken)
        {
            var HomeWorkSubmitions = await _HomeWorkSubmitionRepository.GetByIdAsync(request.HomeWorkSubmitionId);
            return _mapper.Map<GetAllHomeWorkSubmitionsViewModel>(HomeWorkSubmitions);
        }
    }
}
