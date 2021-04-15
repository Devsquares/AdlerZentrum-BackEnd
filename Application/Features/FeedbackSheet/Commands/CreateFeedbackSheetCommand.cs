using Application.Enums;
using Application.Features;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateFeedbackSheetCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public int TestTypeId { get; set; }
        public int Duration { get; set; }
        public List<CreateQuestionCommand> Questions { get; set; }

        public class CreateFeedbackSheetCommandHandler : IRequestHandler<CreateFeedbackSheetCommand, Response<int>>
        {
            private readonly ITestRepositoryAsync _TestRepository;
            private readonly ISingleQuestionRepositoryAsync _singleQuestionRepository;
            private readonly IQuestionRepositoryAsync _questionRepository;
            private readonly IMediator _mediator;

            public CreateFeedbackSheetCommandHandler(
                ISingleQuestionRepositoryAsync singleQuestionRepository,
                IQuestionRepositoryAsync questionRepository,
                ITestRepositoryAsync TestRepository,
                IMediator mediator)
            {
                _TestRepository = TestRepository;
                _singleQuestionRepository = singleQuestionRepository;
                _questionRepository = questionRepository;
                _mediator = mediator;
            }
            public async Task<Response<int>> Handle(CreateFeedbackSheetCommand command, CancellationToken cancellationToken)
            {
                var res = await _mediator.Send(new CheckFeedbackSheetCreationQuery());
                if (res == false) throw new Exception("There is an active Feedback Sheet.");
                var test = new Test();
                test.Name = command.Name;
                test.TestDuration = command.Duration;
                test.TestTypeId = command.TestTypeId;
                test.TotalPoint = 0;
                test.PlacementStartDate = null;

                test = await _TestRepository.AddAsync(test);
                foreach (var item in command.Questions)
                {
                    item.TestId = test.Id;
                    await _mediator.Send(item);
                }

                return new Response<int>(test.Id);
            }
        }
    }
}
