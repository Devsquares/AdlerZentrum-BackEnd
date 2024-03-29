﻿using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllSublevelsQuery : IRequest<IEnumerable<GetAllSubLevelsViewModel>>
    {
    }
    public class GetAllSublevelsQueryHandler : IRequestHandler<GetAllSublevelsQuery, IEnumerable<GetAllSubLevelsViewModel>>
    {
        private readonly ISublevelRepositoryAsync _sublevelService;
        private readonly IMapper _mapper;
        public GetAllSublevelsQueryHandler(ISublevelRepositoryAsync levelService, IMapper mapper)
        {
            _sublevelService = levelService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllSubLevelsViewModel>> Handle(GetAllSublevelsQuery request, CancellationToken cancellationToken)
        {
            var SubLevels = await _sublevelService.GetAllAsync("Level");
            var userViewModel = _mapper.Map<IEnumerable<GetAllSubLevelsViewModel>>(SubLevels);
            return userViewModel;
        }
    }
}
