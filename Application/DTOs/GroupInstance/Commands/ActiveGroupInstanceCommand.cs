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
            private readonly IInterestedStudentRepositoryAsync _interestedStudentRepositoryAsync;

            public ActiveGroupInstanceCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository,
                ILessonInstanceRepositoryAsync lessonInstanceRepository,
                ILessonInstanceStudentRepositoryAsync lessonInstanceStudent,
                ITestRepositoryAsync testRepositoryAsync,
                ITestInstanceRepositoryAsync testInstanceRepository,
                ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignment,
                IGroupInstanceStudentRepositoryAsync groupInstanceStudentRepositoryAsync,
                IUsersRepositoryAsync usersRepositoryAsync,
                ISublevelRepositoryAsync sublevelRepositoryAsync,
                IMailJobRepositoryAsync jobRepositoryAsync,
                 IInterestedStudentRepositoryAsync interestedStudentRepositoryAsync)
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
                _interestedStudentRepositoryAsync =  interestedStudentRepositoryAsync;
            }  public async Task<Response<bool>> Handle(ActiveGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var groupInstance = _groupInstanceRepositoryAsync.GetByIdAsync(command.GroupInstanceId).Result;
                if (groupInstance == null)
                {
                    throw new Exception("This Group instance not found");
                }
                if (groupInstance.Status == (int)GroupInstanceStatusEnum.Pending)
                {
                    // check if there another active group 
                    Test subLevelTest = new Test();
                    Test finalLevelTest = new Test();

                    int? IsOtherActiveGroupInTheGroupDef = _groupInstanceRepositoryAsync.IsOtherActiveGroupInTheGroupDef(groupInstance.GroupDefinitionId);
                    if (IsOtherActiveGroupInTheGroupDef != null)
                    {
                        // get tests from other group.
                        subLevelTest = _testInstanceRepository.GetSubLevelTestByGroupInstance(IsOtherActiveGroupInTheGroupDef.Value);
                        finalLevelTest = _testInstanceRepository.GetFinalLevelTestByGroupInstance(IsOtherActiveGroupInTheGroupDef.Value);
                    }
                    else
                    {
                        // random select tests.    
                        subLevelTest = _testRepository.GetSubLevelTestBySublevelAsync(groupInstance.GroupDefinition.SubLevelId).Result;
                        finalLevelTest = _testRepository.GetFinalLevelTestBySublevelAsync(groupInstance.GroupDefinition.Sublevel.LevelId).Result;
                    }


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
                    var LessonDefinitions = groupInstance.GroupDefinition.Sublevel.LessonDefinitions.OrderBy(x => x.Order);
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

                        CheckAndDeleteInterested(item);
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
                            StartDate = timeslotsDetailed.GetValueOrDefault(item.Order % groupInstance.GroupDefinition.Sublevel.NumberOflessons).Start,
                            EndDate = timeslotsDetailed.GetValueOrDefault(item.Order % groupInstance.GroupDefinition.Sublevel.NumberOflessons).End,
                            DueDate = timeslotsDetailed.GetValueOrDefault(item.Order % groupInstance.GroupDefinition.Sublevel.NumberOflessons).End.AddDays(1),
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
                        Test quiz = _testRepository.GetQuizzByLessonDefinationAsync(item.LessonDefinitionId).Result;
                        if (IsOtherActiveGroupInTheGroupDef != null)
                        {
                            // get tests from other group.
                            quiz = _testInstanceRepository.GetQuizTestByGroupInstanceByLessonDef(IsOtherActiveGroupInTheGroupDef.Value, item.LessonDefinitionId);
                        }
                        else
                        {
                            // random select tests.    
                            quiz = _testRepository.GetQuizzByLessonDefinationAsync(item.LessonDefinitionId).Result;
                        }
                        if (quiz == null) continue;
                        foreach (var student in lessonInstanceStudents)
                        {
                            TestInstance obj = new TestInstance
                            {
                                LessonInstanceId = item.Id,
                                StudentId = student.StudentId,
                                Status = (int)TestInstanceEnum.Closed,
                                TestId = quiz.Id,
                                CorrectionTeacherId = teacher?.TeacherId,
                                GroupInstanceId = groupInstance.Id,
                                StartDate = item.StartDate
                            };
                            testInstance.Add(obj);
                        }
                        if (quiz.Status == (int)TestStatusEnum.Draft)
                        {
                            quiz.Status = (int)TestStatusEnum.Final;
                            await _testRepository.UpdateAsync(quiz);
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
                                    GroupInstanceId = groupInstance.Id,
                                    StartDate = groupInstance.GroupDefinition.FinalTestDate.Value
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
                                    GroupInstanceId = groupInstance.Id,
                                    StartDate = null
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

                    // feedback sheet lesson 4
                    if (lessonInstances[3] != null)
                    {
                        var feedbackSheetLesson4 = await _testRepository.GetFeedbackSheet();
                        if (feedbackSheetLesson4 != null)
                        {
                            foreach (var student in lessonInstanceStudents)
                            {
                                TestInstance obj = new TestInstance
                                {
                                    LessonInstanceId = lessonInstances[3].Id,
                                    StudentId = student.StudentId,
                                    Status = (int)TestInstanceEnum.Closed,
                                    TestId = feedbackSheetLesson4.Id,
                                    CorrectionTeacherId = null,
                                    GroupInstanceId = groupInstance.Id,
                                    StartDate = lessonInstances[3].StartDate
                                };
                                testInstance.Add(obj);
                            }
                        }
                    }


                    if (lessonInstances[7] != null)
                    {
                        var feedbackSheetLesson8 = await _testRepository.GetFeedbackSheet();
                        if (feedbackSheetLesson8 != null)
                        {
                            foreach (var student in lessonInstanceStudents)
                            {
                                TestInstance obj = new TestInstance
                                {
                                    LessonInstanceId = lessonInstances[7].Id,
                                    StudentId = student.StudentId,
                                    Status = (int)TestInstanceEnum.Closed,
                                    TestId = feedbackSheetLesson8.Id,
                                    CorrectionTeacherId = null,
                                    GroupInstanceId = groupInstance.Id,
                                    StartDate = lessonInstances[7].StartDate
                                };
                                testInstance.Add(obj);
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


            private async void CheckAndDeleteInterested(GroupInstanceStudents student)
            {
                if (student.InterestedGroupDefinitionId.HasValue)
                {
                    var interestedstudent = _interestedStudentRepositoryAsync.GetByStudentId(student.StudentId, student.InterestedGroupDefinitionId.Value);
                    if (interestedstudent != null)
                    {
                        await _interestedStudentRepositoryAsync.DeleteAsync(interestedstudent);
                    }
                }
            }
        }
    }
}
