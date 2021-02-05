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

namespace Application.DTOs.GroupInstance.Commands
{
    public class AssignTeacherToGroupInstanceCommand : IRequest<Response<int>>
    {
        public int GroupInstanceId { get; set; }
        public string TeacherId { get; set; }

        public class AssignTeacherToGroupInstanceCommandHandler : IRequestHandler<AssignTeacherToGroupInstanceCommand, Response<int>>
        {
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _groupInstanceRepositoryAsync;
            public AssignTeacherToGroupInstanceCommandHandler(ITeacherGroupInstanceAssignmentRepositoryAsync groupInstanceRepository)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
            }
            public async Task<Response<int>> Handle(AssignTeacherToGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                var obj = _groupInstanceRepositoryAsync.GetByGroupInstanceId(command.GroupInstanceId);
                if (obj == null)
                {
                    obj = new TeacherGroupInstanceAssignment
                    {
                        GroupInstanceId = command.GroupInstanceId,
                        TeacherId = command.TeacherId,
                        IsDefault = true,
                    };
                    await _groupInstanceRepositoryAsync.AddAsync(obj);
                }
                else
                {
                    obj.TeacherId = command.TeacherId;
                    await _groupInstanceRepositoryAsync.UpdateAsync(obj);
                }
                return new Response<int>(obj.Id);
            }
        }
    }
}
