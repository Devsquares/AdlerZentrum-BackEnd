using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TeacherAbsence.Queries.GetTeacherAbsenceById
{
    public class GetTeacherAbsenceByIdQuery : IRequest<Response<Domain.Entities.TeacherAbsence>>
    {
        public int Id { get; set; }
        public class GetTeacherAbsenceByIdQueryHandler : IRequestHandler<GetTeacherAbsenceByIdQuery, Response<Domain.Entities.TeacherAbsence>>
        {
            private readonly ITeacherAbsenceRepositoryAsync _teacherabsenceRepository;
            public GetTeacherAbsenceByIdQueryHandler(ITeacherAbsenceRepositoryAsync teacherabsenceRepository)
            {
                _teacherabsenceRepository = teacherabsenceRepository;
            }
            public async Task<Response<Domain.Entities.TeacherAbsence>> Handle(GetTeacherAbsenceByIdQuery query, CancellationToken cancellationToken)
            {
                var teacherabsence = await _teacherabsenceRepository.GetByIdAsync(query.Id);
                if (teacherabsence == null) throw new ApiException($"TeacherAbsence Not Found.");
                return new Response<Domain.Entities.TeacherAbsence>(teacherabsence);
            }
        }
    }
}
