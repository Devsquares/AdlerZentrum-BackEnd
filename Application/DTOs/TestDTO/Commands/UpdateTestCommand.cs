using Application.Enums;
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
    public class UpdateTestCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TestDuration { get; set; }
        public int TestTypeId { get; set; }
        public List<UpdateQuestionCommand> Questions { get; set; }
        public int? LessonDefinitionId { get; set; }
        public int? SubLevelId { get; set; }
        public int? LevelId { get; set; }

        public class UpdateTestCommandHandler : IRequestHandler<UpdateTestCommand, Response<int>>
        {
            private readonly ITestRepositoryAsync _TestRepository;
            private readonly ISingleQuestionRepositoryAsync _singleQuestionRepository;
            private readonly IQuestionRepositoryAsync _questionRepository;
            private readonly IMediator _mediator;

            public UpdateTestCommandHandler(
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
            public async Task<Response<int>> Handle(UpdateTestCommand command, CancellationToken cancellationToken)
            {
                var test = _TestRepository.GetByIdAsync(command.Id).Result;
                test.Name = command.Name;
                test.TestDuration = command.TestDuration;
                test.TestTypeId = command.TestTypeId;
                test.LessonDefinitionId = command.LessonDefinitionId;
                test.SublevelId = command.SubLevelId;
                test.LevelId = command.LevelId;
                await _TestRepository.UpdateAsync(test);
                
                await _mediator.Send(new RemoveTestFromQuestionsCommand { TestId = command.Id });

                foreach (var item in command.Questions)
                {
                    item.TestId = test.Id;
                    var responaceQuestionId = await _mediator.Send(new UpdateQuestionCommand
                    {
                        Id = item.Id,
                        TestId = item.TestId,
                    });
                    item.Id = responaceQuestionId.data;
                }
                return new Response<int>(test.Id);
            }
        }
    }
}
