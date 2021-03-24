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
            private readonly IMediator _medaitor;
            public UpdateGroupOfSingleQuestionSubmissionHandler(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository,
            ITestInstanceRepositoryAsync testInstanceRepositoryAsync,
            IJobRepositoryAsync jobRepositoryAsync,
            IMediator Mediator)
            {
                _singlequestionsubmissionRepository = singlequestionsubmissionRepository;
                _testInstanceRepository = testInstanceRepositoryAsync;
                _jobRepository = jobRepositoryAsync;
                _medaitor = Mediator;
            }
            public async Task<Response<int>> Handle(UpdateGroupOfSingleQuestionSubmission command, CancellationToken cancellationToken)
            {
                double points = 0;
                int PLACEMENTA1 = 60;
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

                    if (testInstance.Points / testInstance.Test.TotalPoint >= PLACEMENTA1)
                    {
                        
                    }
                }
                await _testInstanceRepository.UpdateAsync(testInstance);

                return new Response<int>(testInstance.Id);
            }
        }

    }
}
