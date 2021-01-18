using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs 
{
    public class GetAllLevelsQuery : IRequest<PagedResponse<IEnumerable<GetAllLevelsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllLevelsQueryHandler : IRequestHandler<GetAllLevelsQuery, PagedResponse<IEnumerable<GetAllLevelsViewModel>>>
    {
        private readonly ILevelRepositoryAsync _levelService;
        private readonly IMapper _mapper;
        public GetAllLevelsQueryHandler(ILevelRepositoryAsync levelService, IMapper mapper)
        {
            _levelService = levelService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllLevelsViewModel>>> Handle(GetAllLevelsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            var user = await _levelService.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize, "SubLevels");
            var userViewModel = _mapper.Map<IEnumerable<GetAllLevelsViewModel>>(user);
            return new PagedResponse<IEnumerable<GetAllLevelsViewModel>>(userViewModel, validFilter.PageNumber, validFilter.PageSize,_levelService.GetCount());
        }
    }
}
