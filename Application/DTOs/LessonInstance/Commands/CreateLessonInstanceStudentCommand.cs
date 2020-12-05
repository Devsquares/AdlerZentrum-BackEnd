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
    public class CreateLessonInstanceStudentCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int LessonInstanceId { get; set; }
        public string StudentId { get; set; }
        public bool Attend { get; set; }
        public bool Homework { get; set; }

        public class CreateLessonInstanceStudentCommandHandler : IRequestHandler<CreateLessonInstanceStudentCommand, Response<int>>
        {
            private readonly ILessonInstanceStudentRepositoryAsync _LessonInstanceStudentRepositoryAsync;
            public CreateLessonInstanceStudentCommandHandler(ILessonInstanceStudentRepositoryAsync LessonInstanceStudentRepository)
            {
                _LessonInstanceStudentRepositoryAsync = LessonInstanceStudentRepository;
            }
            public async Task<Response<int>> Handle(CreateLessonInstanceStudentCommand command, CancellationToken cancellationToken)
            {
                var LessonInstanceStudent = new Domain.Entities.LessonInstanceStudent();

                Reflection.CopyProperties(command, LessonInstanceStudent);
                await _LessonInstanceStudentRepositoryAsync.AddAsync(LessonInstanceStudent);
                return new Response<int>(LessonInstanceStudent.Id);
            }
        }
    }
}
