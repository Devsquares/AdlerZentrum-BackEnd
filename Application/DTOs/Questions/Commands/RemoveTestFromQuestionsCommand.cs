using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RemoveTestFromQuestionsCommand : IRequest<Response<bool>>
    {
        public int TestId { get; set; }

        public class RemoveTestFromQuestionsCommandHandler : IRequestHandler<RemoveTestFromQuestionsCommand, Response<bool>>
        {
            private readonly IQuestionRepositoryAsync _QuestionRepository;
            public RemoveTestFromQuestionsCommandHandler(IQuestionRepositoryAsync QuestionRepository)
            {
                _QuestionRepository = QuestionRepository;
            }
            public async Task<Response<bool>> Handle(RemoveTestFromQuestionsCommand command, CancellationToken cancellationToken)
            {
                var questions = await _QuestionRepository.GetByTestIdAsync(command.TestId);

                if (questions == null)
                {
                    throw new ApiException($"Questions Not Found.");
                }
                else
                {
                    foreach (var item in questions)
                    {
                        item.TestId = null;
                    }

                    await _QuestionRepository.UpdateBulkAsync(questions);
                    return new Response<bool>(true);
                }
            }
        }
    }
}
