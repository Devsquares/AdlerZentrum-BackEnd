using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application
{
    public class AssignAdditionalTeacherToGroupInstanceCommand : IRequest<Response<int>>
    {
        public int GroupInstanceId { get; set; }
        public int LessonInstanceId { get; set; }
        public string TeacherId { get; set; }

        public class AssignAdditionalTeacherToGroupInstanceCommandHandler : IRequestHandler<AssignAdditionalTeacherToGroupInstanceCommand, Response<int>>
        {
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _assignmentRepository;
            public AssignAdditionalTeacherToGroupInstanceCommandHandler(ITeacherGroupInstanceAssignmentRepositoryAsync assignmentRepositoryAsync)
            {
                _assignmentRepository = assignmentRepositoryAsync;
            } 
            public async Task<Response<int>> Handle(AssignAdditionalTeacherToGroupInstanceCommand command, CancellationToken cancellationToken)
            {
                bool isDefault = false;
                var groupInstance = _assignmentRepository.GetListByGroupInstanceId(command.GroupInstanceId); 
                if (groupInstance.Count == 0) isDefault = true;

                TeacherGroupInstanceAssignment obj = new TeacherGroupInstanceAssignment()
                {
                    GroupInstanceId = command.GroupInstanceId,
                    IsDefault = isDefault,
                    LessonInstanceId = command.LessonInstanceId,
                    TeacherId = command.TeacherId
                };
                await _assignmentRepository.AddAsync(obj);
                return new Response<int>(obj.Id);

            }
        }
    }
}
