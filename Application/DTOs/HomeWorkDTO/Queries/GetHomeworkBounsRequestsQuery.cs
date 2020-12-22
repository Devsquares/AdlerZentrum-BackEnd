using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetHomeworkBounsRequestsQuery : IRequest<IEnumerable<GetAllomeworkBounsViewModel>>
    {
    }
    public class GetHomeworkBounsRequestsQueryHandler : IRequestHandler<GetHomeworkBounsRequestsQuery, IEnumerable<GetAllomeworkBounsViewModel>>
    {
        private readonly IHomeWorkRepositoryAsync _HomeWorkRepository;
        private readonly IMapper _mapper;
        public GetHomeworkBounsRequestsQueryHandler(IHomeWorkRepositoryAsync HomeWorkRepository, IMapper mapper)
        {
            _HomeWorkRepository = HomeWorkRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllomeworkBounsViewModel>> Handle(GetHomeworkBounsRequestsQuery request, CancellationToken cancellationToken)
        {
            var HomeWorks = _HomeWorkRepository.GetAllBounsRequests();
            var userViewModel = _mapper.Map<IEnumerable<GetAllomeworkBounsViewModel>>(HomeWorks);
            return userViewModel;
        }
    }
}
