using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllSubLevelsQuery : IRequest<IEnumerable<GetAllSubLevelsViewModel>>
    {
    }
    public class GetAllSubLevelsQueryHandler : IRequestHandler<GetAllSubLevelsQuery, IEnumerable<GetAllSubLevelsViewModel>>
    {
        private readonly ISublevelRepositoryAsync _sublevelService;
        private readonly IMapper _mapper;
        public GetAllSubLevelsQueryHandler(ISublevelRepositoryAsync levelService, IMapper mapper)
        {
            _sublevelService = levelService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllSubLevelsViewModel>> Handle(GetAllSubLevelsQuery request, CancellationToken cancellationToken)
        {
            var SubLevels = await _sublevelService.GetAllAsync("Level");
            var userViewModel = _mapper.Map<IEnumerable<GetAllSubLevelsViewModel>>(SubLevels);
            return userViewModel;
        }
    }
}
