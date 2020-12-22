using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateLessonDefinitionCommand : IRequest<Response<int>>
    {

        public int Id { get; set; }
        public string Name { get; set; } 

        public class UpdateLessonDefinitionCommandHandler : IRequestHandler<UpdateLessonDefinitionCommand, Response<int>>
        {
            private readonly ILessonDefinitionRepositoryAsync _LessonDefinitionRepository;
            public UpdateLessonDefinitionCommandHandler(ILessonDefinitionRepositoryAsync LessonDefinitionRepository)
            {
                _LessonDefinitionRepository = LessonDefinitionRepository;
            }
            public async Task<Response<int>> Handle(UpdateLessonDefinitionCommand command, CancellationToken cancellationToken)
            {
                var LessonDefinition = await _LessonDefinitionRepository.GetByIdAsync(command.Id);

                if (LessonDefinition == null)
                {
                    throw new ApiException($"LessonDefinition Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, LessonDefinition);
                    await _LessonDefinitionRepository.UpdateAsync(LessonDefinition);
                    return new Response<int>(LessonDefinition.Id);
                }
            }
        }
    }
}
