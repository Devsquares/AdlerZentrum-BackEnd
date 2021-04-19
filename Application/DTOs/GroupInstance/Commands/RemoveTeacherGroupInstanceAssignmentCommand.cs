using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class RemoveTeacherGroupInstanceAssignmentCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }

        public class RemoveTeacherGroupInstanceAssignmentCommandHandler : IRequestHandler<RemoveTeacherGroupInstanceAssignmentCommand, Response<bool>>
        {
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _assignmentRepository;
            public RemoveTeacherGroupInstanceAssignmentCommandHandler(ITeacherGroupInstanceAssignmentRepositoryAsync assignmentRepositoryAsync)
            {
                _assignmentRepository = assignmentRepositoryAsync;
            }
            public async Task<Response<bool>> Handle(RemoveTeacherGroupInstanceAssignmentCommand command, CancellationToken cancellationToken)
            {
                var teacherGroupInstanceAssignment = await _assignmentRepository.GetByIdAsync(command.Id);
                if (teacherGroupInstanceAssignment.IsDefault)
                {
                    var nextTeacher = _assignmentRepository.GetFirstNotIsDefault(teacherGroupInstanceAssignment.GroupInstanceId);
                    if (nextTeacher == null) throw new Exception("Please add another teacher first.");
                    nextTeacher.IsDefault = true;
                    await _assignmentRepository.UpdateAsync(nextTeacher);
                } 

                await _assignmentRepository.DeleteAsync(teacherGroupInstanceAssignment);
                return new Response<bool>(true);

            }
        }
    }
}
