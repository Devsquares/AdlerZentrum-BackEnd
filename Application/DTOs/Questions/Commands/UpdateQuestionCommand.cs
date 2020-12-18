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
        public int QuestionTypeId { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public int MinCharacters { get; set; }
        public string AudioPath { get; set; } 

        public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Response<int>>
        {
            private readonly IQuestionRepositoryAsync _QuestionRepository;
            public UpdateQuestionCommandHandler(IQuestionRepositoryAsync QuestionRepository)
            {
                _QuestionRepository = QuestionRepository;
            }
            public async Task<Response<int>> Handle(UpdateQuestionCommand command, CancellationToken cancellationToken)
            {
                var Question = await _QuestionRepository.GetByIdAsync(command.Id);

                if (Question == null)
                {
                    throw new ApiException($"Question Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, Question);
                    await _QuestionRepository.UpdateAsync(Question);
                    return new Response<int>(Question.Id);
                }
            }
        }
    }
}
