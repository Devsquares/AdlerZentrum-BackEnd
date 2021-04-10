using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TeacherAbsence.Commands.UpdateTeacherAbsence
{
	public class UpdateTeacherAbsenceCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public string TeacherId { get; set; }
		//public ApplicationUser Teacher { get; set; }
		public int LessonInstanceId { get; set; }
		//public LessonInstance LessonInstance { get; set; }
		public bool IsEmergency { get; set; }
		public int Status { get; set; }

        public class UpdateTeacherAbsenceCommandHandler : IRequestHandler<UpdateTeacherAbsenceCommand, Response<int>>
        {
            private readonly ITeacherAbsenceRepositoryAsync _teacherabsenceRepository;
            public UpdateTeacherAbsenceCommandHandler(ITeacherAbsenceRepositoryAsync teacherabsenceRepository)
            {
                _teacherabsenceRepository = teacherabsenceRepository;
            }
            public async Task<Response<int>> Handle(UpdateTeacherAbsenceCommand command, CancellationToken cancellationToken)
            {
                var teacherabsence = await _teacherabsenceRepository.GetByIdAsync(command.Id);

                if (teacherabsence == null)
                {
                    throw new ApiException($"TeacherAbsence Not Found.");
                }
                else
                {
				teacherabsence.TeacherId = command.TeacherId;
				//teacherabsence.Teacher = command.Teacher;
				teacherabsence.LessonInstanceId = command.LessonInstanceId;
				//teacherabsence.LessonInstance = command.LessonInstance;
				teacherabsence.IsEmergency = command.IsEmergency;
				teacherabsence.Status = command.Status; 

                    await _teacherabsenceRepository.UpdateAsync(teacherabsence);
                    return new Response<int>(teacherabsence.Id);
                }
            }
        }

    }
}
