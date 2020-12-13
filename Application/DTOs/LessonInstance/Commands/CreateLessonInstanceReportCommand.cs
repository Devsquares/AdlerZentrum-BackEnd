using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateLessonInstanceReportCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public int MaterialDone { get; set; }
        public int MaterialToDo { get; set; }
        public List<LessonInstanceStudentInputModel> LessonInstanceStudent { get; set; }

        public class CreateLessonInstanceReportCommandHandler : IRequestHandler<CreateLessonInstanceReportCommand, Response<bool>>
        {
            private readonly ILessonInstanceRepositoryAsync _LessonInstanceRepositoryAsync;
            private readonly IMediator _mediator;
            public CreateLessonInstanceReportCommandHandler(ILessonInstanceRepositoryAsync LessonInstanceRepository,
                IMediator mediator)
            {
                _LessonInstanceRepositoryAsync = LessonInstanceRepository;
                _mediator = mediator;
            }
            public async Task<Response<bool>> Handle(CreateLessonInstanceReportCommand command, CancellationToken cancellationToken)
            {
                var LessonInstance = new LessonInstance();

                Reflection.CopyProperties(command, LessonInstance);
                await _LessonInstanceRepositoryAsync.UpdateAsync(LessonInstance);
                await _mediator.Send(new CreateLessonInstanceStudentCommand { inputModels = command.LessonInstanceStudent });
                return new Response<bool>(true);
            }
        }
    }
}
