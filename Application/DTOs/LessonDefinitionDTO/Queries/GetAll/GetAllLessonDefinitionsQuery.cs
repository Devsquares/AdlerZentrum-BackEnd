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
    public class GetAllLessonDefinitionsQuery : IRequest<PagedResponse<IEnumerable<GetAllLessonDefinitionsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllLessonDefinitionsQueryHandler : IRequestHandler<GetAllLessonDefinitionsQuery, PagedResponse<IEnumerable<GetAllLessonDefinitionsViewModel>>>
    {
        private readonly ILessonDefinitionRepositoryAsync _LessonDefinitionService;
        private readonly IMapper _mapper;
        public GetAllLessonDefinitionsQueryHandler(ILessonDefinitionRepositoryAsync LessonDefinitionService, IMapper mapper)
        {
            _LessonDefinitionService = LessonDefinitionService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllLessonDefinitionsViewModel>>> Handle(GetAllLessonDefinitionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            var user = await _LessonDefinitionService.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var userViewModel = _mapper.Map<IEnumerable<GetAllLessonDefinitionsViewModel>>(user);
            return new PagedResponse<IEnumerable<GetAllLessonDefinitionsViewModel>>(userViewModel, validFilter.PageNumber, validFilter.PageSize,_LessonDefinitionService.GetCount());
        }
    }
}
