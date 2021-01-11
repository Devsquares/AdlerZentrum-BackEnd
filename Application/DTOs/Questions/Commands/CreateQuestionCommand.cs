using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateQuestionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int? TestId { get; set; }
        public int QuestionTypeId { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public int MinCharacters { get; set; }
        public string AudioPath { get; set; }
        public int NoOfRepeats { get; set; }
        public virtual ICollection<SingleQuestionCreateInputModel> SingleQuestions { get; set; }

        public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Response<int>>
        {
            private readonly IQuestionRepositoryAsync _QuestionRepository;
            private readonly IMediator _mediator;
            public CreateQuestionCommandHandler(IQuestionRepositoryAsync QuestionRepository,
                IMediator mediator)
            {
                _QuestionRepository = QuestionRepository;
                _mediator = mediator;
            }
            public async Task<Response<int>> Handle(CreateQuestionCommand command, CancellationToken cancellationToken)
            {
                var question = new Question();

                Reflection.CopyProperties(command, question);
                await _QuestionRepository.AddAsync(question);
                foreach (var item in command.SingleQuestions)
                {
                    await _mediator.Send(new UpdateSingleQuestionCommand { Id = item.Id, QuestionId = question.Id });
                }
                return new Response<int>(question.Id);
            }
        }
    }
}
