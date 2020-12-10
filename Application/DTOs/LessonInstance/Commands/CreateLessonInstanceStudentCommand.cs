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
        public List<LessonInstanceStudentInputModel> inputModels { get; set; }

        public class CreateLessonInstanceStudentCommandHandler : IRequestHandler<CreateLessonInstanceStudentCommand, Response<bool>>
        {
            private readonly ILessonInstanceStudentRepositoryAsync _LessonInstanceStudentRepositoryAsync;
            public CreateLessonInstanceStudentCommandHandler(ILessonInstanceStudentRepositoryAsync LessonInstanceStudentRepository)
            {
                _LessonInstanceStudentRepositoryAsync = LessonInstanceStudentRepository;
            }
            public async Task<Response<bool>> Handle(CreateLessonInstanceStudentCommand command, CancellationToken cancellationToken)
            {
                var LessonInstanceStudent = new List<LessonInstanceStudent>();

                Reflection.CopyProperties(command, LessonInstanceStudent);
                await _LessonInstanceStudentRepositoryAsync.AddBulkAsync(LessonInstanceStudent);
                return new Response<bool>(true);
            }
        }
    }
}
