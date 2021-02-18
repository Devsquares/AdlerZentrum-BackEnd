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
            private readonly IMediator _medaitor;
            public UpdateGroupOfSingleQuestionSubmissionHandler(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository,
            ITestInstanceRepositoryAsync testInstanceRepositoryAsync,
            IMediator Mediator)
            {
                _singlequestionsubmissionRepository = singlequestionsubmissionRepository;
                _testInstanceRepository = testInstanceRepositoryAsync;
                _medaitor = Mediator;
            }
            public async Task<Response<int>> Handle(UpdateGroupOfSingleQuestionSubmission command, CancellationToken cancellationToken)
            {
                int points = 0;
                foreach (var item in command.SingleQuestionSubmission)
                {
                    await _medaitor.Send(item);
                    if (item.RightAnswer)
                    {
                        points = points + item.Points;
                    }
                }

                var testInstance = _testInstanceRepository.GetByIdAsync(command.TestInstanceId).Result;
                testInstance.Status = (int)TestInstanceEnum.Corrected;
                testInstance.Points = testInstance.Points + points;
                await _testInstanceRepository.UpdateAsync(testInstance);

                return new Response<int>(testInstance.Id);
            }
        }

    }
}
