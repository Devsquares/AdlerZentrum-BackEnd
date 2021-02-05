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
    public class EditTeacherToGroupInstanceCommand : IRequest<Response<int>>
    {
        public int NewGroupInstanceId { get; set; }
        public int OldGroupInstanceId { get; set; }
        public string TeacherId { get; set; }

        public class EditTeacherToGroupInstanceCommandHandler : IRequestHandler<EditTeacherToGroupInstanceCommand, Response<int>>
        {
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroupInstanceAssignmentRepositoryAsync;
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            public EditTeacherToGroupInstanceCommandHandler(ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignmentRepositoryAsync,
                IGroupInstanceRepositoryAsync groupInstanceRepository)
            {
                _teacherGroupInstanceAssignmentRepositoryAsync = teacherGroupInstanceAssignmentRepositoryAsync;
                _groupInstanceRepositoryAsync = groupInstanceRepository;
            }
            public async Task<Response<int>> Handle(EditTeacherToGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var groupINstance = _groupInstanceRepositoryAsync.GetByIdAsync(command.NewGroupInstanceId).Result;
                if (groupINstance == null)
                {
                    throw new ApiException("Group Instance not found");
                }
                var teacherGroupInstance = _teacherGroupInstanceAssignmentRepositoryAsync.GetByTeachGroupInstanceId(command.TeacherId, command.OldGroupInstanceId);
                if (teacherGroupInstance == null)
                {
                    throw new ApiException("No teacher for this group instance");
                }
                teacherGroupInstance.GroupInstanceId = command.NewGroupInstanceId;
                await _teacherGroupInstanceAssignmentRepositoryAsync.UpdateAsync(teacherGroupInstance);

                return new Response<int>(teacherGroupInstance.Id);
            }
        }
    }
}
