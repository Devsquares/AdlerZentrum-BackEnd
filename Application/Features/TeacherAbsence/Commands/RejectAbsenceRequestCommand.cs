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
    public partial class RejectAbsenceRequestCommand : IRequest<Response<int>>
    {
       // public string TeacherId { get; set; }
        public int AbsenceRequestId { get; set; }
    }

    public class RejectAbsenceRequestCommandHandler : IRequestHandler<RejectAbsenceRequestCommand, Response<int>>
    {
        private readonly ITeacherAbsenceRepositoryAsync _teacherabsenceRepository;
        private readonly IMapper _mapper;
        private readonly IMailJobRepositoryAsync _jobRepository;
        public RejectAbsenceRequestCommandHandler(ITeacherAbsenceRepositoryAsync teacherabsenceRepository, IMapper mapper,
            IMailJobRepositoryAsync jobRepository)
        {
            _teacherabsenceRepository = teacherabsenceRepository;
            _mapper = mapper;
            _jobRepository = jobRepository;
        }

        public async Task<Response<int>> Handle(RejectAbsenceRequestCommand request, CancellationToken cancellationToken)
        {

            var teacherabsence = _teacherabsenceRepository.GetbyId(request.AbsenceRequestId);
            if (teacherabsence == null)
            {
                throw new ApiException("Teacher Absence object not found");
            }
            teacherabsence.Status = (int)TeacherAbsenceStatusEnum.Rejected;
            await _teacherabsenceRepository.UpdateAsync(teacherabsence);
            await _jobRepository.AddAsync(new Domain.Entities.MailJob
            {
                Type = (int)MailJobTypeEnum.RejectTeacherAbsence,
                GroupInstanceId = teacherabsence.LessonInstance.GroupInstanceId,
                TeacherId = teacherabsence.TeacherId,
                Status = (int)JobStatusEnum.New
            });
            return new Response<int>(teacherabsence.Id);
        }
    }
}
