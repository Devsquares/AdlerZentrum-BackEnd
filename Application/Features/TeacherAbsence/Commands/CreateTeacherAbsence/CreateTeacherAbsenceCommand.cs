using Application.Enums;
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
    public partial class CreateTeacherAbsenceCommand : IRequest<Response<int>>
    {
		public string TeacherId { get; set; }
		//public ApplicationUser Teacher { get; set; }
		public int LessonInstanceId { get; set; }
		//public LessonInstance LessonInstance { get; set; }
		public bool IsEmergency { get; set; }
		//public int Status { get; set; }
    }

    public class CreateTeacherAbsenceCommandHandler : IRequestHandler<CreateTeacherAbsenceCommand, Response<int>>
    {
        private readonly ITeacherAbsenceRepositoryAsync _teacherabsenceRepository;
        private readonly IMapper _mapper;
        public CreateTeacherAbsenceCommandHandler(ITeacherAbsenceRepositoryAsync teacherabsenceRepository, IMapper mapper)
        {
            _teacherabsenceRepository = teacherabsenceRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateTeacherAbsenceCommand request, CancellationToken cancellationToken)
        {
            var teacherabsence = _mapper.Map<Domain.Entities.TeacherAbsence>(request);
            teacherabsence.Status = (int)TeacherAbsenceStatusEnum.New;
            await _teacherabsenceRepository.AddAsync(teacherabsence);
            return new Response<int>(teacherabsence.Id);
        }
    }
}
