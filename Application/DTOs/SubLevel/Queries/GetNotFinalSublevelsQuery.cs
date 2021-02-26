using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetNotFinalSublevelsQuery : IRequest<IEnumerable<GetAllSubLevelsViewModel>>
    {
    }
    public class GetNotFinalSublevelsQueryHandler : IRequestHandler<GetNotFinalSublevelsQuery, IEnumerable<GetAllSubLevelsViewModel>>
    {
        private readonly ISublevelRepositoryAsync _levelService;
        private readonly IMapper _mapper;
        public GetNotFinalSublevelsQueryHandler(ISublevelRepositoryAsync levelService, IMapper mapper)
        {
            _levelService = levelService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllSubLevelsViewModel>> Handle(GetNotFinalSublevelsQuery request, CancellationToken cancellationToken)
        {
            var SubLevels = _levelService.GetNotFinalSublevels();
            var userViewModel = _mapper.Map<IEnumerable<GetAllSubLevelsViewModel>>(SubLevels);
            return userViewModel;
        }
    }
}
