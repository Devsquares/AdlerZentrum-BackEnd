using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllHomeWorkSubmitionsForStudentQuery : IRequest<IEnumerable<GetAllHomeWorkForStudentViewModel>>
    {
        public string StudentId { get; set; }
        public int GroupInstanceId { get; set; }
    }
    public class GetAllHomeWorkSubmitionsForStudentQueryHandler : IRequestHandler<GetAllHomeWorkSubmitionsForStudentQuery, IEnumerable<GetAllHomeWorkForStudentViewModel>>
    {
        private readonly IHomeWorkSubmitionRepositoryAsync _HomeWorkSubmitionRepository;
        private readonly IMapper _mapper;
        public GetAllHomeWorkSubmitionsForStudentQueryHandler(IHomeWorkSubmitionRepositoryAsync HomeWorkSubmitionRepository, IMapper mapper)
        {
            _HomeWorkSubmitionRepository = HomeWorkSubmitionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllHomeWorkForStudentViewModel>> Handle(GetAllHomeWorkSubmitionsForStudentQuery request, CancellationToken cancellationToken)
        { 
            var HomeWorkSubmitions = await _HomeWorkSubmitionRepository.GetAllForStudentAsync(request.StudentId, request.GroupInstanceId);
            var userViewModel = _mapper.Map<IEnumerable<GetAllHomeWorkForStudentViewModel>>(HomeWorkSubmitions);
            return userViewModel;
        }
    }
}
