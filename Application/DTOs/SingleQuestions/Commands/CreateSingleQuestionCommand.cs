using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateSingleQuestionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        public int SingleQuestionType { get; set; }
        public string Text { get; set; }
        public ICollection<Choice> Choices { get; set; }

        public class CreateSingleQuestionCommandHandler : IRequestHandler<CreateSingleQuestionCommand, Response<int>>
        {
            private readonly ISingleQuestionRepositoryAsync _SingleQuestionRepository;
            public CreateSingleQuestionCommandHandler(ISingleQuestionRepositoryAsync SingleQuestionRepository)
            {
                _SingleQuestionRepository = SingleQuestionRepository;
            }
            public async Task<Response<int>> Handle(CreateSingleQuestionCommand command, CancellationToken cancellationToken)
            {
                var SingleQuestion = new Domain.Entities.SingleQuestion();

                Reflection.CopyProperties(command, SingleQuestion);
                await _SingleQuestionRepository.AddAsync(SingleQuestion);
                return new Response<int>(SingleQuestion.Id);

            }
        }
    }
}
