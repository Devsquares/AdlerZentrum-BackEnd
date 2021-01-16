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
    public class UpdateQuestionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int? TestId { get; set; }

        public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Response<int>>
        {
            private readonly IQuestionRepositoryAsync _QuestionRepository;
            public UpdateQuestionCommandHandler(IQuestionRepositoryAsync QuestionRepository)
            {
                _QuestionRepository = QuestionRepository;
            }
            public async Task<Response<int>> Handle(UpdateQuestionCommand command, CancellationToken cancellationToken)
            {
                var question = await _QuestionRepository.GetByIdAsync(command.Id);

                if (question == null)
                {
                    throw new ApiException($"Question Not Found.");
                }
                else
                {
                    question.TestId = command.TestId;
                    await _QuestionRepository.UpdateAsync(question);
                    return new Response<int>(question.Id);
                }
            }
        }
    }
}
