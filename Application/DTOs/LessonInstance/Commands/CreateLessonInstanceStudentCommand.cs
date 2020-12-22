using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateLessonInstanceStudentCommand : IRequest<Response<bool>>
    {
        public List<LessonInstanceStudent> inputModels { get; set; }

        public class CreateLessonInstanceStudentCommandHandler : IRequestHandler<CreateLessonInstanceStudentCommand, Response<bool>>
        {
            private readonly ILessonInstanceStudentRepositoryAsync _LessonInstanceStudentRepositoryAsync;
            public CreateLessonInstanceStudentCommandHandler(ILessonInstanceStudentRepositoryAsync LessonInstanceStudentRepository)
            {
                _LessonInstanceStudentRepositoryAsync = LessonInstanceStudentRepository;
            }
            public async Task<Response<bool>> Handle(CreateLessonInstanceStudentCommand command, CancellationToken cancellationToken)
            { 
                await _LessonInstanceStudentRepositoryAsync.UpdateBulkAsync(command.inputModels);
                return new Response<bool>(true);
            }
        }
    }
}
