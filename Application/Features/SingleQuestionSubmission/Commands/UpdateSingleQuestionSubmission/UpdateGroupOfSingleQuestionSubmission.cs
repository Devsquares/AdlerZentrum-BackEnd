using Application.Enums;
using Application.Exceptions;
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
    public class UpdateGroupOfSingleQuestionSubmission : IRequest<Response<int>>
    {
        public List<UpdateSingleQuestionSubmissionCommand> SingleQuestionSubmission { get; set; }
        public int TestInstanceId { get; set; }

        public class UpdateGroupOfSingleQuestionSubmissionHandler : IRequestHandler<UpdateGroupOfSingleQuestionSubmission, Response<int>>
        {
            private readonly ISingleQuestionSubmissionRepositoryAsync _singlequestionsubmissionRepository;
            private readonly ITestInstanceRepositoryAsync _testInstanceRepository;
            private readonly IJobRepositoryAsync _jobRepository;
            private readonly ISettingRepositoryAsync _settings;
            private readonly ISublevelRepositoryAsync _sublevel;
            private readonly IUsersRepositoryAsync _usersRepositoryAsync;
            private readonly IMailJobRepositoryAsync _jobMailRepository;
            private readonly IMediator _medaitor;
            public UpdateGroupOfSingleQuestionSubmissionHandler(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository,
            ITestInstanceRepositoryAsync testInstanceRepositoryAsync,
            IJobRepositoryAsync jobRepositoryAsync,
            ISettingRepositoryAsync settingRepositoryAsync,
            ISublevelRepositoryAsync sublevelRepositoryAsync,
            IUsersRepositoryAsync usersRepositoryAsync,
            IMailJobRepositoryAsync mailJobRepositoryAsync,
            IMediator Mediator)
            {
                _singlequestionsubmissionRepository = singlequestionsubmissionRepository;
                _testInstanceRepository = testInstanceRepositoryAsync;
                _jobRepository = jobRepositoryAsync;
                _settings = settingRepositoryAsync;
                _sublevel = sublevelRepositoryAsync;
                _usersRepositoryAsync = usersRepositoryAsync;
                _jobMailRepository = mailJobRepositoryAsync;
                _medaitor = Mediator;
            }
            public async Task<Response<int>> Handle(UpdateGroupOfSingleQuestionSubmission command, CancellationToken cancellationToken)
            {
                double points = 0;
                var settings = _settings.GetAllAsync().Result;
                var testInstanceId = command.TestInstanceId;
                foreach (var item in command.SingleQuestionSubmission)
                {
                    var d = await _medaitor.Send(item);
                    testInstanceId = d.data;
                }

                var testInstance = await _testInstanceRepository.GetByIdAsync(testInstanceId);
                var singleQuestions = await _singlequestionsubmissionRepository.GetByTestInstanceIdAsync(testInstance.Id);

                foreach (var item in singleQuestions)
                {
                    if (item.RightAnswer)
                    {
                        points = points + item.Points;
                    }
                }
                testInstance.Status = (int)TestInstanceEnum.Corrected;
                testInstance.CorrectionDate = DateTime.Now;

                await _jobRepository.AddAsync(new Job
                {
                    Type = (int)JobTypeEnum.ScoreCalculator,
                    StudentId = testInstance.StudentId,
                    Status = (int)JobStatusEnum.New
                });
                await _jobMailRepository.AddAsync(new MailJob
                {
                    Type = (int)MailJobTypeEnum.TestCorrected,
                    TestInstanceId = testInstance.Id,
                    StudentId = testInstance.StudentId,
                    Status = (int)JobStatusEnum.New
                });
                testInstance.Points = points;
                if (testInstance.Test.TestTypeId == (int)TestTypeEnum.placement)
                {
                    //TODO: set the sublevel of the student.
                    if (settings.Count > 0)
                    {
                        double precetnge = (testInstance.Points / testInstance.Test.TotalPoint) / 100;
                        var user = _usersRepositoryAsync.GetUserById(testInstance.StudentId);
                        Sublevel sublevel = null;
                        // TODO: set the right order.

                        //(x >= 1 && x <= 100)
                        if (precetnge >= settings[0].PlacementC1)
                        {
                            sublevel = _sublevel.GetByOrder(13);
                        }
                        else if (precetnge >= settings[0].PlacementB2 && precetnge < settings[0].PlacementC1)
                        {
                            sublevel = _sublevel.GetByOrder(10);
                        }
                        else if (precetnge >= settings[0].PlacementB1 && precetnge < settings[0].PlacementB2)
                        {
                            sublevel = _sublevel.GetByOrder(7);
                        }
                        else if (precetnge >= settings[0].PlacementA2 && precetnge < settings[0].PlacementB1)
                        {
                            sublevel = _sublevel.GetByOrder(4);
                        }
                        else
                        {
                            sublevel = _sublevel.GetByOrder(1);
                        }

                        user.SublevelId = sublevel.Id;
                        await _usersRepositoryAsync.UpdateAsync(user);
                    }
                }
                await _testInstanceRepository.UpdateAsync(testInstance);

                return new Response<int>(testInstance.Id);
            }
        }

    }
}
