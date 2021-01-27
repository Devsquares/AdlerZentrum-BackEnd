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
        public string SubLevel { get; set; }
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
            int totalCount = 0;
            var validFilter = _mapper.Map<RequestParameter>(request);
            IReadOnlyList<Domain.Entities.GroupDefinition> GroupDefinitions;
            GroupDefinitions =  _GroupDefinitionRepositoryAsync.GetALL(request.PageNumber, request.PageSize,request.SubLevel,out totalCount);
            var groupDefinitionsModel = _mapper.Map<IEnumerable<GetAllGroupDefinitionViewModel>>(GroupDefinitions);
            return new PagedResponse<IEnumerable<GetAllGroupDefinitionViewModel>>(groupDefinitionsModel, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
