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
        public int SingleQuestionTypeId { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public int MinCharacters { get; set; }
        public string AudioPath { get; set; } 

        public class UpdateSingleQuestionCommandHandler : IRequestHandler<UpdateSingleQuestionCommand, Response<int>>
        {
            private readonly ISingleQuestionRepositoryAsync _SingleQuestionRepository;
            public UpdateSingleQuestionCommandHandler(ISingleQuestionRepositoryAsync SingleQuestionRepository)
            {
                _SingleQuestionRepository = SingleQuestionRepository;
            }
            public async Task<Response<int>> Handle(UpdateSingleQuestionCommand command, CancellationToken cancellationToken)
            {
                var SingleQuestion = await _SingleQuestionRepository.GetByIdAsync(command.Id);

                if (SingleQuestion == null)
                {
                    throw new ApiException($"SingleQuestion Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, SingleQuestion);
                    await _SingleQuestionRepository.UpdateAsync(SingleQuestion);
                    return new Response<int>(SingleQuestion.Id);
                }
            }
        }
    }
}
