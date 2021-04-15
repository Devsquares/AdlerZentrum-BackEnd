using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TeacherAbsence.Commands.CreateTeacherAbsence
{
    public partial class AcceptAbsenceRequestCommand : IRequest<Response<int>>
    {
		public string TeacherId { get; set; }
		public int AbsenceRequestId { get; set; }
    }

    public class AcceptAbsenceRequestCommandHandler : IRequestHandler<AcceptAbsenceRequestCommand, Response<int>>
    {
        private readonly ITeacherAbsenceRepositoryAsync _teacherabsenceRepository;
        private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroupInstanceAssignmentRepository;
        private readonly IMapper _mapper;
        private readonly IMailJobRepositoryAsync _jobRepository;
        public AcceptAbsenceRequestCommandHandler(ITeacherAbsenceRepositoryAsync teacherabsenceRepository, IMapper mapper,
            ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignmentRepository, IMailJobRepositoryAsync jobRepository)
        {
            _teacherabsenceRepository = teacherabsenceRepository;
            _mapper = mapper;
            _teacherGroupInstanceAssignmentRepository = teacherGroupInstanceAssignmentRepository;
            _jobRepository = jobRepository;
        }

        public async Task<Response<int>> Handle(AcceptAbsenceRequestCommand request, CancellationToken cancellationToken)
        {

            var teacherabsence = _teacherabsenceRepository.GetbyId(request.AbsenceRequestId);
            if(teacherabsence == null)
            {
                throw new ApiException("Teacher Absence object not found");
            }
            teacherabsence.Status = (int)TeacherAbsenceStatusEnum.Accepted;
            await _teacherabsenceRepository.UpdateAsync(teacherabsence);
            await _teacherGroupInstanceAssignmentRepository.AddAsync(new Domain.Entities.TeacherGroupInstanceAssignment()
            {
                TeacherId = request.TeacherId,
                GroupInstanceId = teacherabsence.LessonInstance.GroupInstanceId,
                LessonInstanceId = teacherabsence.LessonInstanceId,
                CreatedDate = DateTime.Now,
                IsDefault = false

            });
            // old Teacher
            await _jobRepository.AddAsync(new Domain.Entities.MailJob
            {
                Type = (int)MailJobTypeEnum.AcceptTeacherAbsence,
                GroupInstanceId = teacherabsence.LessonInstance.GroupInstanceId,
                TeacherId = teacherabsence.TeacherId,
                Status = (int)JobStatusEnum.New
            });
            // new teacher
            await _jobRepository.AddAsync(new Domain.Entities.MailJob
            {
                Type = (int)MailJobTypeEnum.AcceptTeacherAbsenceWithAnotherTeacher,
                GroupInstanceId = teacherabsence.LessonInstance.GroupInstanceId,
                TeacherId = request.TeacherId,
                Status = (int)JobStatusEnum.New
            });

            return new Response<int>(teacherabsence.Id);
        }
    }
}
