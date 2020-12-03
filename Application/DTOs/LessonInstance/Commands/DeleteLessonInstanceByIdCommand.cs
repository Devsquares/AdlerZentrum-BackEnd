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
    public class DeleteLessonInstanceByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteLessonInstanceByIdCommandHandler : IRequestHandler<DeleteLessonInstanceByIdCommand, Response<int>>
        {
            private readonly ILessonInstanceRepositoryAsync _LessonInstanceRepositoryAsync;
            public DeleteLessonInstanceByIdCommandHandler(ILessonInstanceRepositoryAsync LessonInstanceRepository)
            {
                _LessonInstanceRepositoryAsync = LessonInstanceRepository;
            }
            public async Task<Response<int>> Handle(DeleteLessonInstanceByIdCommand command, CancellationToken cancellationToken)
            {
                var LessonInstance = await _LessonInstanceRepositoryAsync.GetByIdAsync(command.Id);
                if (LessonInstance == null) throw new ApiException($"Group Not Found.");
                await _LessonInstanceRepositoryAsync.DeleteAsync(LessonInstance);
                return new Response<int>(LessonInstance.Id);
            }
        }
    }
}
