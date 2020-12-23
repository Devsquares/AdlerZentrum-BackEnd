using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllGroupDefinitionsQuery : IRequest<PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllGroupDefinitionsQueryHandler : IRequestHandler<GetAllGroupDefinitionsQuery, PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>>
    {
        private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllGroupDefinitionsQueryHandler(IGroupDefinitionRepositoryAsync GroupDefinitionService, IMapper mapper)
        {
            _GroupDefinitionRepositoryAsync = GroupDefinitionService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>> Handle(GetAllGroupDefinitionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            IReadOnlyList<Domain.Entities.GroupDefinition> GroupDefinitions;
            GroupDefinitions = await _GroupDefinitionRepositoryAsync.GetPagedReponseAsync(request.PageNumber, request.PageSize);
            var userViewModel = _mapper.Map<IEnumerable<GetAllGroupDefinitionViewModel>>(GroupDefinitions);
            return new PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>(userViewModel, request.PageNumber, request.PageSize);
        }
    }
}
