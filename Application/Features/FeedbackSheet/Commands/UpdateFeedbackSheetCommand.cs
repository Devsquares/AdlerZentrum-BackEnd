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
    public class UpdateFeedbackSheetCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TestTypeId { get; set; }
        public int Duration { get; set; }
        public List<CreateQuestionCommand> Questions { get; set; }

        public class CreateFeedbackSheetCommandHandler : IRequestHandler<UpdateFeedbackSheetCommand, Response<int>>
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
            public async Task<Response<int>> Handle(UpdateFeedbackSheetCommand command, CancellationToken cancellationToken)
            {
                var test = await _TestRepository.GetByIdAsync(command.Id);
                if (test.Status != (int)TestStatusEnum.Draft) return new Response<int>("Test status not equal draft.");
                test.Name = command.Name;
                test.TestDuration = command.Duration;
                test.TestTypeId = command.TestTypeId;
                test.TotalPoint = 0;
                test.PlacementStartDate = null;

                await _TestRepository.UpdateAsync(test);
                foreach (var item in command.Questions)
                {
                    await _mediator.Send(new DeleteQuestionByIdCommand()
                    {
                        Id = item.Id
                    });
                }
                foreach (var item in command.Questions)
                {
                    await _mediator.Send(new CreateQuestionCommand()
                    {
                        Header = item.Header,
                        AudioPath = item.AudioPath,
                        MinCharacters = item.MinCharacters,
                        NoOfRepeats = item.NoOfRepeats,
                        Order = item.Order,
                        QuestionTypeId = item.QuestionTypeId,
                        SingleQuestions = item.SingleQuestions,
                        TestId = test.Id,
                        Text = item.Text
                    });
                }

                return new Response<int>(test.Id);
            }
        }
    }
}
