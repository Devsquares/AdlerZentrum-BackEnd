using Application.Enums;
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
        public string MaterialDone { get; set; }
        public string MaterialToDo { get; set; }
        public bool isAdditionalHomework { get; set; }
        public CreateHomeWorkCommand AdditionalHomework { get; set; }
        public List<LessonInstanceStudent> LessonInstanceStudent { get; set; }
        public string TeacherId { get; set; }

        public class CreateLessonInstanceReportCommandHandler : IRequestHandler<CreateLessonInstanceReportCommand, Response<bool>>
        {
            private readonly ILessonInstanceRepositoryAsync _LessonInstanceRepositoryAsync;
            private readonly IJobRepositoryAsync _jobRepository;
            private readonly IMediator _mediator;
            public CreateLessonInstanceReportCommandHandler(ILessonInstanceRepositoryAsync LessonInstanceRepository,
            IJobRepositoryAsync jobRepositoryAsync,
                IMediator mediator)
            {
                _LessonInstanceRepositoryAsync = LessonInstanceRepository;
                _jobRepository = jobRepositoryAsync;
                _mediator = mediator;
            }
            public async Task<Response<bool>> Handle(CreateLessonInstanceReportCommand command, CancellationToken cancellationToken)
            {
                var lessonInstance = new LessonInstance();
                lessonInstance = _LessonInstanceRepositoryAsync.GetByIdAsync(command.Id).Result;
                if (!lessonInstance.SubmittedReport)
                {
                    lessonInstance.MaterialDone = command.MaterialDone;
                    lessonInstance.MaterialToDo = command.MaterialToDo;
                    lessonInstance.SubmittedReport = true;

                    await _LessonInstanceRepositoryAsync.UpdateAsync(lessonInstance);
                    foreach (var item in command.LessonInstanceStudent)
                    {
                        item.LessonInstanceId = lessonInstance.Id;
                    }
                    await _mediator.Send(new CreateLessonInstanceStudentCommand { inputModels = command.LessonInstanceStudent });

                    if (command.isAdditionalHomework)
                    {
                        await _mediator.Send(new CreateHomeWorkCommand
                        {
                            BonusPoints = command.AdditionalHomework.BonusPoints,
                            GroupInstanceId = lessonInstance.GroupInstanceId,
                            MinCharacters = command.AdditionalHomework.MinCharacters,
                            Points = command.AdditionalHomework.Points,
                            Text = command.AdditionalHomework.Text,
                            TeacherId = command.TeacherId,
                            LessonInstanceId = lessonInstance.Id
                        });
                    }
                    foreach (var item in lessonInstance.LessonInstanceStudents)
                    {
                        await _jobRepository.AddAsync(new Job
                        {
                            Type = (int)JobTypeEnum.Upgrader,
                            StudentId = item.StudentId,
                            Status = (int)JobStatusEnum.New
                        });
                    }
                }
                return new Response<bool>(true);
            }
        }
    }
}
