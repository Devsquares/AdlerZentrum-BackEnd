using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetLessonDefinitionByIdQuery : IRequest<Response<Domain.Entities.LessonDefinition>>
    {
        public int Id { get; set; }
        public class GetLessonDefinitionByIdQueryHandler : IRequestHandler<GetLessonDefinitionByIdQuery, Response<Domain.Entities.LessonDefinition>>
        {
            private readonly ILessonDefinitionRepositoryAsync _LessonDefinitionService;
            public GetLessonDefinitionByIdQueryHandler(ILessonDefinitionRepositoryAsync LessonDefinitionService)
            {
                _LessonDefinitionService = LessonDefinitionService;
            }
            public async Task<Response<Domain.Entities.LessonDefinition>> Handle(GetLessonDefinitionByIdQuery query, CancellationToken cancellationToken)
            {
                var LessonDefinition = await _LessonDefinitionService.GetByIdAsync(query.Id);
                if (LessonDefinition == null) throw new ApiException($"LessonDefinition Not Found.");
                return new Response<Domain.Entities.LessonDefinition>(LessonDefinition);
            }
        }
    }
}
