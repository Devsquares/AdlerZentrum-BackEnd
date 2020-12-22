using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DeleteLessonDefinitionByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteLessonDefinitionByIdCommandHandler : IRequestHandler<DeleteLessonDefinitionByIdCommand, Response<int>>
        {
            private readonly ILessonDefinitionRepositoryAsync _LessonDefinitionRepositoryAsync;
            public DeleteLessonDefinitionByIdCommandHandler(ILessonDefinitionRepositoryAsync LessonDefinitionRepositoryAsync)
            {
                _LessonDefinitionRepositoryAsync = LessonDefinitionRepositoryAsync;
            }
            public async Task<Response<int>> Handle(DeleteLessonDefinitionByIdCommand command, CancellationToken cancellationToken)
            {
                var LessonDefinition = await _LessonDefinitionRepositoryAsync.GetByIdAsync(command.Id);
                if (LessonDefinition == null) throw new ApiException($"LessonDefinition Not Found.");
                await _LessonDefinitionRepositoryAsync.DeleteAsync(LessonDefinition);
                return new Response<int>(LessonDefinition.Id);
            }
        }
    }
}
