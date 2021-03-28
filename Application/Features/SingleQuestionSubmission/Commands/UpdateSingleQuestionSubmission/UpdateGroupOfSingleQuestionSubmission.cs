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
            private readonly IMediator _medaitor;
            public UpdateGroupOfSingleQuestionSubmissionHandler(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository,
            ITestInstanceRepositoryAsync testInstanceRepositoryAsync,
            IJobRepositoryAsync jobRepositoryAsync,
            ISettingRepositoryAsync settingRepositoryAsync,
            ISublevelRepositoryAsync sublevelRepositoryAsync,
            IUsersRepositoryAsync usersRepositoryAsync,
            IMediator Mediator)
            {
                _singlequestionsubmissionRepository = singlequestionsubmissionRepository;
                _testInstanceRepository = testInstanceRepositoryAsync;
                _jobRepository = jobRepositoryAsync;
                _settings = settingRepositoryAsync;
                _sublevel = sublevelRepositoryAsync;
                _usersRepositoryAsync = usersRepositoryAsync;
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
                    if (item.RightAnswer)
                    {
                        points = points + item.Points;
                    }
                }

                var testInstance = _testInstanceRepository.GetByIdAsync(testInstanceId).Result;
                testInstance.Status = (int)TestInstanceEnum.Corrected;
                await _jobRepository.AddAsync(new Job
                {
                    Type = (int)JobTypeEnum.ScoreCalculator,
                    StudentId = testInstance.StudentId,
                    Status = (int)JobStatusEnum.New
                });
                testInstance.Points = testInstance.Points + points;
                if (testInstance.Test.TestTypeId == (int)TestTypeEnum.placement)
                {
                    //TODO: set the sublevel of the student.
                    if (settings.Count > 0)
                    {
                        double precetnge = (testInstance.Points / testInstance.Test.TotalPoint) / 100;
                        var user = _usersRepositoryAsync.GetUserById(testInstance.StudentId);
                        Sublevel sublevel = null;
                        if (precetnge >= settings[0].PlacementB2)
                        {
                            // TODO: fix this
                            // A1 50 % -A2 60 % -B1 70 % -B2 80 %
                            sublevel = _sublevel.GetByOrder(4);
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
