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

namespace Application.Features.teacherActions.Commands
{
    public class AssignTeacherToGroupInstanceCommand : IRequest<Response<int>>
    {
        public int GroupInstanceId { get; set; }
        public string TeacherId { get; set; }

        public class AssignTeacherToGroupInstanceCommandHandler : IRequestHandler<AssignTeacherToGroupInstanceCommand, Response<int>>
        {
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroupInstanceAssignmentRepositoryAsync;
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            public AssignTeacherToGroupInstanceCommandHandler(ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignmentRepositoryAsync,
                IGroupInstanceRepositoryAsync groupInstanceRepository)
            {
                _teacherGroupInstanceAssignmentRepositoryAsync = teacherGroupInstanceAssignmentRepositoryAsync;
                _groupInstanceRepositoryAsync = groupInstanceRepository;
            }
            public async Task<Response<int>> Handle(AssignTeacherToGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var groupINstance = _groupInstanceRepositoryAsync.GetByIdAsync(command.GroupInstanceId).Result;
                if (groupINstance == null)
                {
                    throw new ApiException("Group Instance not found");
                }
               var  teacherGroupInstance= new TeacherGroupInstanceAssignment
                {
                    GroupInstanceId = command.GroupInstanceId,
                    TeacherId = command.TeacherId,
                    IsDefault = true,
                    CreatedDate = DateTime.Now
                };
                await _teacherGroupInstanceAssignmentRepositoryAsync.AddAsync(teacherGroupInstance);

                return new Response<int>(teacherGroupInstance.Id);
            }
        }
    }
}
