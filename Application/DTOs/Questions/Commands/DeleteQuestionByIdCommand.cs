using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DeleteQuestionByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteQuestionByIdCommandHandler : IRequestHandler<DeleteQuestionByIdCommand, Response<int>>
        {
            private readonly IQuestionRepositoryAsync _QuestionRepositoryAsync;
            public DeleteQuestionByIdCommandHandler(IQuestionRepositoryAsync QuestionRepositoryAsync)
            {
                _QuestionRepositoryAsync = QuestionRepositoryAsync;
            }
            public async Task<Response<int>> Handle(DeleteQuestionByIdCommand command, CancellationToken cancellationToken)
            {
                var Question = await _QuestionRepositoryAsync.GetByIdAsync(command.Id);
                if (Question == null) throw new ApiException($"Question Not Found.");
                await _QuestionRepositoryAsync.DeleteAsync(Question);
                return new Response<int>(Question.Id);
            }
        }
    }
}
