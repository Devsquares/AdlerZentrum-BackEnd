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
    public class DeleteSingleQuestionByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteSingleQuestionByIdCommandHandler : IRequestHandler<DeleteSingleQuestionByIdCommand, Response<int>>
        {
            private readonly ISingleQuestionRepositoryAsync _SingleQuestionRepositoryAsync;
            public DeleteSingleQuestionByIdCommandHandler(ISingleQuestionRepositoryAsync SingleQuestionRepositoryAsync)
            {
                _SingleQuestionRepositoryAsync = SingleQuestionRepositoryAsync;
            }
            public async Task<Response<int>> Handle(DeleteSingleQuestionByIdCommand command, CancellationToken cancellationToken)
            {
                var SingleQuestion = await _SingleQuestionRepositoryAsync.GetByIdAsync(command.Id);
                if (SingleQuestion == null) throw new ApiException($"SingleQuestion Not Found.");
                await _SingleQuestionRepositoryAsync.DeleteAsync(SingleQuestion);
                return new Response<int>(SingleQuestion.Id);
            }
        }
    }
}
