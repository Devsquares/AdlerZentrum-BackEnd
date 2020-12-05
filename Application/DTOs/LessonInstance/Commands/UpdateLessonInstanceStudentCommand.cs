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
    public class UpdateLessonInstanceStudentCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int LessonInstanceId { get; set; }
        public int StudentId { get; set; }
        public bool Attend { get; set; }
        public bool Homework { get; set; }

        public class UpdateLessonInstanceStudentCommandHandler : IRequestHandler<UpdateLessonInstanceStudentCommand, Response<int>>
        {
            private readonly ILessonInstanceStudentRepositoryAsync _LessonInstanceStudentRepositoryAsync;
            public UpdateLessonInstanceStudentCommandHandler(ILessonInstanceStudentRepositoryAsync LessonInstanceStudentRepository)
            {
                _LessonInstanceStudentRepositoryAsync = LessonInstanceStudentRepository;
            }
            public async Task<Response<int>> Handle(UpdateLessonInstanceStudentCommand command, CancellationToken cancellationToken)
            {
                var LessonInstanceStudent = await _LessonInstanceStudentRepositoryAsync.GetByIdAsync(command.Id);

                if (LessonInstanceStudent == null)
                {
                    throw new ApiException($"Group Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, LessonInstanceStudent);
                    await _LessonInstanceStudentRepositoryAsync.UpdateAsync(LessonInstanceStudent);
                    return new Response<int>(LessonInstanceStudent.Id);
                }
            }
        }
    }
}
