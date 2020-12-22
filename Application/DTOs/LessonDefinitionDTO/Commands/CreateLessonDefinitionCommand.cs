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
    public class CreateLessonDefinitionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; } 

        public class CreateLessonDefinitionCommandHandler : IRequestHandler<CreateLessonDefinitionCommand, Response<int>>
        {
            private readonly ILessonDefinitionRepositoryAsync _LessonDefinitionRepository;
            public CreateLessonDefinitionCommandHandler(ILessonDefinitionRepositoryAsync LessonDefinitionRepository)
            {
                _LessonDefinitionRepository = LessonDefinitionRepository;
            }
            public async Task<Response<int>> Handle(CreateLessonDefinitionCommand command, CancellationToken cancellationToken)
            {
                var LessonDefinition = new Domain.Entities.LessonDefinition();

                Reflection.CopyProperties(command, LessonDefinition);
                await _LessonDefinitionRepository.AddAsync(LessonDefinition);
                return new Response<int>(LessonDefinition.Id);

            }
        }
    }
}
