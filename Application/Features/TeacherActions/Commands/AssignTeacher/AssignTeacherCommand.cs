using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class AssignTeacherCommand : IRequest<Response<bool>>
    {
        public string TeacherId { get; set; }
        public int? TestInstanceId { get; set; }
        public int? LessonInstanceId { get; set; }
        public int? GroupInstanceId { get; set; }

        public class AssignTeacherCommandHandler : IRequestHandler<AssignTeacherCommand, Response<bool>>
        {
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroupInstanceAssignmentRepositoryAsync;
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly ITestInstanceRepositoryAsync _testInstanceRepository;
            public AssignTeacherCommandHandler(
                ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignmentRepositoryAsync,
                IGroupInstanceRepositoryAsync groupInstanceRepository,
                ITestInstanceRepositoryAsync testInstanceRepository)
            {
                _teacherGroupInstanceAssignmentRepositoryAsync = teacherGroupInstanceAssignmentRepositoryAsync;
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _testInstanceRepository = testInstanceRepository;
            }
            public async Task<Response<bool>> Handle(AssignTeacherCommand command, CancellationToken cancellationToken)
            {
                List<TestInstance> data = new List<TestInstance>();
                if (command.TestInstanceId.HasValue)
                {
                    data.Add(await _testInstanceRepository.GetByIdAsync(command.TestInstanceId.Value));
                }

                if (command.LessonInstanceId.HasValue)
                {
                    data = await _testInstanceRepository.GetTestInstanceByLessonInstanceId(command.LessonInstanceId.Value);
                }

                if (command.GroupInstanceId.HasValue)
                {
                    data = await _testInstanceRepository.GetAllTestInstancesByGroup(command.GroupInstanceId.Value);
                }

                foreach (var item in data)
                {
                    item.CorrectionTeacherId = command.TeacherId;
                }
                await _testInstanceRepository.UpdateBulkAsync(data);

                return new Response<bool>();
            }
        }
    }
}
