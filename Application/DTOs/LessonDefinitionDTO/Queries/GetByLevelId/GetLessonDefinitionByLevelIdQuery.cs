using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetLessonDefinitionByLevelIdQuery : IRequest<Response<ICollection<LessonDefinition>>>
    {
        public int SubLevelId { get; set; }
        public class GetLessonDefinitionByLevelIdQueryHandler : IRequestHandler<GetLessonDefinitionByLevelIdQuery, Response<ICollection<LessonDefinition>>>
        {
            private readonly ILessonDefinitionRepositoryAsync _LessonDefinitionService;
            public GetLessonDefinitionByLevelIdQueryHandler(ILessonDefinitionRepositoryAsync LessonDefinitionService)
            {
                _LessonDefinitionService = LessonDefinitionService;
            }
            public async Task<Response<ICollection<LessonDefinition>>> Handle(GetLessonDefinitionByLevelIdQuery query, CancellationToken cancellationToken)
            {
                var LessonDefinition = await _LessonDefinitionService.GetBySubLevelId(query.SubLevelId);
                if (LessonDefinition == null) throw new ApiException($"Lesson Definitions Not Found.");
                return new Response<ICollection<LessonDefinition>>(LessonDefinition);
            }
        }
    }
}
