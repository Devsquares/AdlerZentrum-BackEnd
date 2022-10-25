using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetHomeworkBounsRequestsQuery : IRequest<PagedResponse<IEnumerable<GetAllomeworkBounsViewModel>>>
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int? status { get; set; }

        public class GetHomeworkBounsRequestsQueryHandler : IRequestHandler<GetHomeworkBounsRequestsQuery, PagedResponse<IEnumerable<GetAllomeworkBounsViewModel>>>
        {
            private readonly IHomeWorkRepositoryAsync _HomeWorkRepository;
            private readonly IMapper _mapper;
            public GetHomeworkBounsRequestsQueryHandler(IHomeWorkRepositoryAsync HomeWorkRepository, IMapper mapper)
            {
                _HomeWorkRepository = HomeWorkRepository;
                _mapper = mapper;
            }

            public async Task<PagedResponse<IEnumerable<GetAllomeworkBounsViewModel>>> Handle(GetHomeworkBounsRequestsQuery request, CancellationToken cancellationToken)
            {
                if (request.pageNumber == 0) request.pageNumber = 1;
                if (request.pageSize == 0) request.pageSize = 10;
                int count = _HomeWorkRepository.GetAllBounsRequestsCount(request.status);
                var HomeWorks = _HomeWorkRepository.GetAllBounsRequests(request.pageNumber, request.pageSize, request.status);
                var userViewModel = _mapper.Map<IEnumerable<GetAllomeworkBounsViewModel>>(HomeWorks);
                return new PagedResponse<IEnumerable<GetAllomeworkBounsViewModel>>(userViewModel, request.pageNumber, request.pageSize, count);
            }
        }
    }
}
