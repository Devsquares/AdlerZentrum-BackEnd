﻿using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllHomeWorkSubmitionsQuery : IRequest<IEnumerable<GetAllHomeWorkSubmitionsViewModel>>
    {
        public int? GroupInstanceId { get; set; }
        public string TeacherId { get; set; }
        public int? Status { get; set; }
    }
    public class GetAllHomeWorkSubmitionsQueryHandler : IRequestHandler<GetAllHomeWorkSubmitionsQuery, IEnumerable<GetAllHomeWorkSubmitionsViewModel>>
    {
        private readonly IHomeWorkSubmitionRepositoryAsync _HomeWorkSubmitionRepository;
        private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroupInstanceAssignment;
        private readonly IMapper _mapper;
        public GetAllHomeWorkSubmitionsQueryHandler(IHomeWorkSubmitionRepositoryAsync HomeWorkSubmitionRepository,
            ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignment,
            IMapper mapper)
        {
            _HomeWorkSubmitionRepository = HomeWorkSubmitionRepository;
            _teacherGroupInstanceAssignment = teacherGroupInstanceAssignment;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllHomeWorkSubmitionsViewModel>> Handle(GetAllHomeWorkSubmitionsQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<HomeWorkSubmition> homeWorkSubmitions = null;
            if (request.GroupInstanceId == null)
            {
                var res = _teacherGroupInstanceAssignment.GetByTeacherId(request.TeacherId);
                if (res.LessonInstanceId == null)
                {
                    homeWorkSubmitions = await _HomeWorkSubmitionRepository.GetAllByTeacherIdAsync(request.TeacherId, request.Status);
                }
                else
                {
                    homeWorkSubmitions = await _HomeWorkSubmitionRepository.GetAllByLessonIdAsync(res.LessonInstanceId.Value, request.Status);
                }
            }
            else
            {
                homeWorkSubmitions = await _HomeWorkSubmitionRepository.GetAllByGroupInstanceAsync(request.GroupInstanceId.Value, request.Status);
            }
            var userViewModel = _mapper.Map<IEnumerable<GetAllHomeWorkSubmitionsViewModel>>(homeWorkSubmitions);
            return userViewModel;
        }
    }
}
