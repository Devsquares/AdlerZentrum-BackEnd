using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetLessonDefinitionByLevelIdQuery : IRequest<Response<ICollection<GetLessonDefinitionByLevelIdViewModel>>>
    {
        public int SubLevelId { get; set; }
        public class GetLessonDefinitionByLevelIdQueryHandler : IRequestHandler<GetLessonDefinitionByLevelIdQuery, Response<ICollection<GetLessonDefinitionByLevelIdViewModel>>>
        {
            private readonly ILessonDefinitionRepositoryAsync _LessonDefinitionService;
            private readonly IMapper _mapper;
            public GetLessonDefinitionByLevelIdQueryHandler(ILessonDefinitionRepositoryAsync LessonDefinitionService, IMapper mapper)
            {
                _LessonDefinitionService = LessonDefinitionService;
                _mapper = mapper;
            }
            public async Task<Response<ICollection<GetLessonDefinitionByLevelIdViewModel>>> Handle(GetLessonDefinitionByLevelIdQuery query, CancellationToken cancellationToken)
            {
                var LessonDefinition = await _LessonDefinitionService.GetBySubLevelId(query.SubLevelId);
                if (LessonDefinition == null) throw new ApiException($"Lesson Definitions Not Found.");

                var LessonDefinitionViewModel = _mapper.Map<ICollection<GetLessonDefinitionByLevelIdViewModel>>(LessonDefinition);
                return new Response<ICollection<GetLessonDefinitionByLevelIdViewModel>>(LessonDefinitionViewModel);
            }
        }
    }
}
