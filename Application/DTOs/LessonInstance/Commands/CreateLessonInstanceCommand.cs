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
    public class CreateLessonInstanceCommand : IRequest<Response<int>>
    {
        public int GroupDefinitionId { get; set; }
        public int Serail { get; set; }
        public int? Status { get; set; }

        public class CreateLessonInstanceCommandHandler : IRequestHandler<CreateLessonInstanceCommand, Response<int>>
        {
            private readonly ILessonInstanceRepositoryAsync _LessonInstanceRepositoryAsync;
            public CreateLessonInstanceCommandHandler(ILessonInstanceRepositoryAsync LessonInstanceRepository)
            {
                _LessonInstanceRepositoryAsync = LessonInstanceRepository;
            }
            public async Task<Response<int>> Handle(CreateLessonInstanceCommand command, CancellationToken cancellationToken)
            {
                var LessonInstance = new Domain.Entities.LessonInstance();

                Reflection.CopyProperties(command, LessonInstance);
                await _LessonInstanceRepositoryAsync.AddAsync(LessonInstance);
                return new Response<int>(LessonInstance.Id);

            }
        }
    }
}
