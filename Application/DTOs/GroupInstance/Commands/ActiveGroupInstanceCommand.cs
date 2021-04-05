using Application.Enums;
using Application.Exceptions;
using Application.Helpers;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ActiveGroupInstanceCommand : IRequest<Response<bool>>
    {
        public int GroupInstanceId { get; set; }

        public class ActiveGroupInstanceCommandHandler : IRequestHandler<ActiveGroupInstanceCommand, Response<bool>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IGroupInstanceStudentRepositoryAsync _groupInstanceStudentRepositoryAsync;
            private readonly ILessonInstanceRepositoryAsync _lessonInstanceRepositoryAsync;
            private readonly ILessonInstanceStudentRepositoryAsync _lessonInstanceStudentRepositoryAsync;
            private readonly ITestRepositoryAsync _testRepository;
            private readonly ITestInstanceRepositoryAsync _testInstanceRepository;
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroupInstanceAssignment;
            private readonly IUsersRepositoryAsync _usersRepositoryAsync;
            private readonly ISublevelRepositoryAsync _sublevelRepositoryAsync;
            private readonly IMailJobRepositoryAsync _jobRepository;

            public ActiveGroupInstanceCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
                ILessonInstanceRepositoryAsync lessonInstanceRepository,
                ILessonInstanceStudentRepositoryAsync lessonInstanceStudent,
                ITestRepositoryAsync testRepositoryAsync,
                ITestInstanceRepositoryAsync testInstanceRepository,
                ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignment,
                IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
                IUsersRepositoryAsync usersRepositoryAsync,
                ISublevelRepositoryAsync sublevelRepositoryAsync,
                IMailJobRepositoryAsync jobRepositoryAsync)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
                _lessonInstanceRepositoryAsync = lessonInstanceRepository;
                _lessonInstanceStudentRepositoryAsync = lessonInstanceStudent;
                _testRepository = testRepositoryAsync;
                _testInstanceRepository = testInstanceRepository;
                _teacherGroupInstanceAssignment = teacherGroupInstanceAssignment;
                _groupInstanceStudentRepositoryAsync = groupInstanceStudentRepositoryAsync;
                _usersRepositoryAsync = usersRepositoryAsync;
                _sublevelRepositoryAsync = sublevelRepositoryAsync;
                _jobRepository = jobRepositoryAsync;
            }

            public async Task<Response<bool>> Handle(ActiveGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var groupInstance = _groupInstanceRepositoryAsync.GetByIdAsync(command.GroupInstanceId).Result;
                if (groupInstance == null)
                {
                    throw new Exception("This Group instance not found");
                }
                if (groupInstance.Status == (int)GroupInstanceStatusEnum.Pending)
                {
                    var subLevelTest = _testRepository.GetSubLevelTestBySublevelAsync(groupInstance.GroupDefinition.SubLevelId).Result;
                    var finalLevelTest = _testRepository.GetFinalLevelTestBySublevelAsync(groupInstance.GroupDefinition.Sublevel.LevelId).Result;

                    var notIsEligablStudents = groupInstance.Students.Where(x => x.IsEligible == false).ToList();
                    if (notIsEligablStudents != null && notIsEligablStudents.Count() > 0)
                    {
                        throw new Exception("This Group instance cannot be activated because some students are not Eligible ");
                    }
                    var teacher = _teacherGroupInstanceAssignment.GetByGroupInstanceId(groupInstance.Id);
                    if (teacher == null)
                    {
                        throw new Exception("This Group instance not acctive yet kindly choose the teacher.");
                    }
                    else
                    {
                        await _jobRepository.AddAsync(new MailJob
                        {
                            Type = (int)MailJobTypeEnum.GroupActivationTeacher,
                            GroupInstanceId = groupInstance.Id,
                            TeacherId = teacher.TeacherId,
                            Status = (int)JobStatusEnum.New
                        });
                    }
                    if (groupInstance.GroupDefinition.Sublevel.IsFinal)
                    {
                        if (finalLevelTest == null) throw new Exception("Cann't active group please create Final test.");
                    }
                    else
                    {
                        if (subLevelTest == null) throw new Exception("Cann't active group please create sublevel test.");
                    }
                    var LessonDefinitions = groupInstance.GroupDefinition.Sublevel.LessonDefinitions.OrderBy(x=>x.Order);
                    // TODO: genrate date time for lesson instance.
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
                        item.IsDefault = true;
                        var student = _usersRepositoryAsync.GetUserById(item.StudentId);
                        student.SublevelId = groupInstance.GroupDefinition.SubLevelId;
                        await _usersRepositoryAsync.UpdateAsync(student);
                        await _jobRepository.AddAsync(new MailJob
                        {
                            Type = (int)MailJobTypeEnum.GroupActivationStudent,
                            StudentId = student.Id,
                            GroupInstanceId = groupInstance.Id,
                            Status = (int)JobStatusEnum.New
                        });
                    }
                    await _groupInstanceStudentRepositoryAsync.UpdateBulkAsync(groupInstance.Students.ToList());

                    List<LessonInstance> lessonInstances = new List<LessonInstance>();
                    Dictionary<int, TimeFrame> timeslotsDetailed = await _lessonInstanceRepositoryAsync.GetTimeSlotInstancesSorted(groupInstance);
                    foreach (var item in LessonDefinitions)
                    {
                        lessonInstances.Add(new LessonInstance
                        {
                            GroupInstanceId = groupInstance.Id,
                            LessonDefinitionId = item.Id,
                            StartDate = timeslotsDetailed.GetValueOrDefault(item.Order).Start,
                            EndDate = timeslotsDetailed.GetValueOrDefault(item.Order).End,
                            MaterialDone = string.Empty,
                            MaterialToDo = string.Empty,
                            Serial = item.Order.ToString()
                        });
                    }
                    groupInstance.Status = (int)GroupInstanceStatusEnum.Running;
                    await _groupInstanceRepositoryAsync.UpdateAsync(groupInstance);
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
                                    TestId = quiz.Id,
                                    CorrectionTeacherId = teacher?.TeacherId,
                                    GroupInstanceId = groupInstance.Id
                                };
                                testInstance.Add(obj);
                            }
                            if (quiz.Status == (int)TestStatusEnum.Draft)
                            {
                                quiz.Status = (int)TestStatusEnum.Final;
                                await _testRepository.UpdateAsync(quiz);
                            }

                        }
                    }

                    if (groupInstance.GroupDefinition.Sublevel.IsFinal)
                    {
                        var lastLesson = lessonInstances[lessonInstances.Count - 1];

                        if (finalLevelTest != null)
                        {
                            foreach (var student in lessonInstanceStudents)
                            {
                                TestInstance obj = new TestInstance
                                {
                                    LessonInstanceId = lastLesson.Id,
                                    StudentId = student.StudentId,
                                    Status = (int)TestInstanceEnum.Closed,
                                    TestId = finalLevelTest.Id,
                                    GroupInstanceId = groupInstance.Id
                                };
                                testInstance.Add(obj);
                            }
                            if (finalLevelTest.Status == (int)TestStatusEnum.Draft)
                            {
                                finalLevelTest.Status = (int)TestStatusEnum.Final;
                                await _testRepository.UpdateAsync(finalLevelTest);
                            }
                        }

                    }
                    else
                    {
                        var lastLesson = lessonInstances[lessonInstances.Count - 1];

                        if (subLevelTest != null)
                        {
                            foreach (var student in lessonInstanceStudents)
                            {
                                TestInstance obj = new TestInstance
                                {
                                    LessonInstanceId = lastLesson.Id,
                                    StudentId = student.StudentId,
                                    Status = (int)TestInstanceEnum.Closed,
                                    TestId = subLevelTest.Id,
                                    CorrectionTeacherId = teacher?.TeacherId,
                                    GroupInstanceId = groupInstance.Id
                                };
                                testInstance.Add(obj);
                            }
                            if (subLevelTest.Status == (int)TestStatusEnum.Draft)
                            {
                                subLevelTest.Status = (int)TestStatusEnum.Final;
                                await _testRepository.UpdateAsync(subLevelTest);
                            }
                        }
                    }
                    await _testInstanceRepository.AddBulkAsync(testInstance);
                }
                else
                {
                    throw new Exception("Cann't active group please check the status.");
                }
                return new Response<bool>(true);
            }
        }
    }
}
