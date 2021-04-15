using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TeacherAbsence.Commands.DeleteTeacherAbsenceById
{
    public class DeleteTeacherAbsenceByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteTeacherAbsenceByIdCommandHandler : IRequestHandler<DeleteTeacherAbsenceByIdCommand, Response<int>>
        {
            private readonly ITeacherAbsenceRepositoryAsync _teacherabsenceRepository;
            public DeleteTeacherAbsenceByIdCommandHandler(ITeacherAbsenceRepositoryAsync teacherabsenceRepository)
            {
                _teacherabsenceRepository = teacherabsenceRepository;
            }
            public async Task<Response<int>> Handle(DeleteTeacherAbsenceByIdCommand command, CancellationToken cancellationToken)
            {
                var teacherabsence = await _teacherabsenceRepository.GetByIdAsync(command.Id);
                if (teacherabsence == null) throw new ApiException($"TeacherAbsence Not Found.");
                await _teacherabsenceRepository.DeleteAsync(teacherabsence);
                return new Response<int>(teacherabsence.Id);
            }
        }
    }
}
