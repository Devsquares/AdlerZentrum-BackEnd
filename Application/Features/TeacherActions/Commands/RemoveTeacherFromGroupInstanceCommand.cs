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

namespace Application.Features
{
    public class RemoveTeacherFromGroupInstanceCommand : IRequest<Response<int>>
    {
        public int GroupInstanceId { get; set; }
        public string TeacherId { get; set; }

        public class RemoveTeacherFromGroupInstanceCommandHandler : IRequestHandler<RemoveTeacherFromGroupInstanceCommand, Response<int>>
        {
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroupInstanceAssignmentRepositoryAsync;
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            public RemoveTeacherFromGroupInstanceCommandHandler(ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignmentRepositoryAsync,
                IGroupInstanceRepositoryAsync groupInstanceRepository)
            {
                _teacherGroupInstanceAssignmentRepositoryAsync = teacherGroupInstanceAssignmentRepositoryAsync;
                _groupInstanceRepositoryAsync = groupInstanceRepository;
            }
            public async Task<Response<int>> Handle(RemoveTeacherFromGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var groupINstance = _groupInstanceRepositoryAsync.GetByIdAsync(command.GroupInstanceId).Result;
                if (groupINstance == null)
                {
                    throw new ApiException("Group Instance not found");
                }
                var teacherGroupInstance = _teacherGroupInstanceAssignmentRepositoryAsync.GetByTeachGroupInstanceId(command.TeacherId, command.GroupInstanceId);
                if(teacherGroupInstance == null)
                {
                    throw new ApiException("No teacher for this group instance");
                }

                await _teacherGroupInstanceAssignmentRepositoryAsync.DeleteAsync(teacherGroupInstance);

                return new Response<int>(teacherGroupInstance.Id);
            }
        }
    }
}
