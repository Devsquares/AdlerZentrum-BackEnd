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

namespace Application.DTOs
{
    public class ActiveGroupInstanceCommand : IRequest<Response<int>>
    {
        public int GroupInstanceId { get; set; }

        public class ActiveGroupInstanceCommandHandler : IRequestHandler<ActiveGroupInstanceCommand, Response<int>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly ILessonInstanceRepositoryAsync _lessonInstanceRepositoryAsync;
            private readonly ILessonInstanceStudentRepositoryAsync _lessonInstanceStudentRepositoryAsync;
            public ActiveGroupInstanceCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
                ILessonInstanceRepositoryAsync lessonInstanceRepository,
                ILessonInstanceStudentRepositoryAsync lessonInstanceStudent)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _lessonInstanceRepositoryAsync = lessonInstanceRepository;
                _lessonInstanceStudentRepositoryAsync = lessonInstanceStudent;
            }

            public async Task<Response<int>> Handle(ActiveGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var groupInstance = _groupInstanceRepositoryAsync.GetByIdAsync(command.GroupInstanceId).Result;
                if (groupInstance.Status == 0)
                {
                    var LessonDefinitions = groupInstance.GroupDefinition.Sublevel.LessonDefinitions;

                    List<LessonInstanceStudent> lessonInstanceStudents = new List<LessonInstanceStudent>();
                    foreach (var item in groupInstance.Students)
                    {
                        lessonInstanceStudents.Add(new LessonInstanceStudent
                        {
                            Attend = true,
                            Homework = true,
                            Student = item.Student,
                            StudentId = item.StudentId
                        });
                    }

                    List<LessonInstance> lessonInstances = new List<LessonInstance>();
                    foreach (var item in LessonDefinitions)
                    { 
                        lessonInstances.Add(new LessonInstance
                        {
                            GroupInstanceId = groupInstance.Id,
                            LessonDefinitionId = item.Id,
                            MaterialDone = string.Empty,
                            MaterialToDo = string.Empty,
                            Serial = item.Order.ToString()
                        });
                    }


                    // TODO: update group status set it with 1 and change it in the next iteration.
                    groupInstance.Status = 1;
                    await _groupInstanceRepositoryAsync.UpdateAsync(groupInstance);
                    // TODO: check before add
                    // await _lessonInstanceRepositoryAsync.DeleteBulkAsync(lessonInstances);
                    await _lessonInstanceRepositoryAsync.AddBulkAsync(lessonInstances);
                    List<LessonInstanceStudent> LessonInstanceStudentsList = new List<LessonInstanceStudent>();

                    foreach (var item in lessonInstances)
                    {
                        for (int i = 0; i < lessonInstanceStudents.Count; i++)
                        {
                            var temp = (LessonInstanceStudent)lessonInstanceStudents[i].Clone();
                            temp.LessonInstanceId = item.Id;
                            LessonInstanceStudentsList.Add(temp);
                        }
                    }
                    await _lessonInstanceStudentRepositoryAsync.UpdateBulkAsync(LessonInstanceStudentsList);
                }
                return new Response<int>(groupInstance.Id);
            }
        }
    }
}
