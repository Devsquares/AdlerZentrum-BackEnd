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
    public class UpdateLessonInstanceCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int GroupDefinitionId { get; set; }
        public int Serial { get; set; }
        public int? Status { get; set; }

        public class UpdateLessonInstanceCommandHandler : IRequestHandler<UpdateLessonInstanceCommand, Response<int>>
        {
            private readonly ILessonInstanceRepositoryAsync _LessonInstanceRepositoryAsync;
            public UpdateLessonInstanceCommandHandler(ILessonInstanceRepositoryAsync LessonInstanceRepository)
            {
                _LessonInstanceRepositoryAsync = LessonInstanceRepository;
            }
            public async Task<Response<int>> Handle(UpdateLessonInstanceCommand command, CancellationToken cancellationToken)
            {
                var LessonInstance = await _LessonInstanceRepositoryAsync.GetByIdAsync(command.Id);

                if (LessonInstance == null)
                {
                    throw new ApiException($"Group Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, LessonInstance);
                    await _LessonInstanceRepositoryAsync.UpdateAsync(LessonInstance);
                    return new Response<int>(LessonInstance.Id);
                }
            }
        }
    }
}
