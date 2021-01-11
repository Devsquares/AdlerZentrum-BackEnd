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
    public class UpdateSingleQuestionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public class UpdateSingleQuestionCommandHandler : IRequestHandler<UpdateSingleQuestionCommand, Response<int>>
        {
            private readonly ISingleQuestionRepositoryAsync _SingleQuestionRepository;
            public UpdateSingleQuestionCommandHandler(ISingleQuestionRepositoryAsync SingleQuestionRepository)
            {
                _SingleQuestionRepository = SingleQuestionRepository;
            }
            public async Task<Response<int>> Handle(UpdateSingleQuestionCommand command, CancellationToken cancellationToken)
            {
                var singleQuestion = await _SingleQuestionRepository.GetByIdAsync(command.Id);

                if (singleQuestion == null)
                {
                    throw new ApiException($"SingleQuestion Not Found.");
                }
                else
                {
                    singleQuestion.QuestionId = command.QuestionId;
                    await _SingleQuestionRepository.UpdateAsync(singleQuestion);
                    return new Response<int>(singleQuestion.Id);
                }
            }
        }
    }
}
