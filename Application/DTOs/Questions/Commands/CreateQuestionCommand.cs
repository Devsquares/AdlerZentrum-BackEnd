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
        public QuestionCreateInputModel Question { get; set; }

        public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Response<int>>
        {
            private readonly IQuestionRepositoryAsync _QuestionRepository;
            public CreateQuestionCommandHandler(IQuestionRepositoryAsync QuestionRepository)
            {
                _QuestionRepository = QuestionRepository;
            }
            public async Task<Response<int>> Handle(CreateQuestionCommand command, CancellationToken cancellationToken)
            {
                var Question = new Domain.Entities.Question();

                Reflection.CopyProperties(command.Question, Question);
                await _QuestionRepository.AddAsync(Question);
                return new Response<int>(Question.Id);

            }
        }
    }
}
