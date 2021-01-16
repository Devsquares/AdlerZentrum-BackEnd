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
            private readonly ITestRepositoryAsync _testRepository;
            private readonly ITestInstanceRepositoryAsync _testInstanceRepository;
            public ActiveGroupInstanceCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
                ILessonInstanceRepositoryAsync lessonInstanceRepository,
                ILessonInstanceStudentRepositoryAsync lessonInstanceStudent,
                ITestRepositoryAsync testRepositoryAsync,
                ITestInstanceRepositoryAsync testInstanceRepository)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _lessonInstanceRepositoryAsync = lessonInstanceRepository;
                _lessonInstanceStudentRepositoryAsync = lessonInstanceStudent;
                _testRepository = testRepositoryAsync;
                _testInstanceRepository = testInstanceRepository;
            }

            public async Task<Response<int>> Handle(ActiveGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var groupInstance = _groupInstanceRepositoryAsync.GetByIdAsync(command.GroupInstanceId).Result;
                if (groupInstance.Status == 0)
                {
                    var LessonDefinitions = groupInstance.GroupDefinition.Sublevel.LessonDefinitions;

                    // TODO: remove it to API..
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

                    List<TestInstance> testInstance = new List<TestInstance>(); 
                    foreach (var item in lessonInstances)
                    {
                        // TODO get quiz for lesson.
                        // TODO add get final test or sublevel test.
                        var quiz = _testRepository.GetQuizzByLessonDefinationAsync(item.LessonDefinitionId).Result;
                        if (quiz != null)
                        {
                            foreach (var student in lessonInstanceStudents)
                            {
                                TestInstance obj = new TestInstance
                                {
                                    LessonInstanceId = item.Id,
                                    StudentId = student.StudentId,
                                    Status = (int)TestInstanceEnum.Closed,
                                    TestId = quiz.Id
                                };
                                testInstance.Add(obj);
                            }
                        }
                    }
                    await _testInstanceRepository.AddBulkAsync(testInstance);
                }
                return new Response<int>(groupInstance.Id);
            }
        }
    }
}
