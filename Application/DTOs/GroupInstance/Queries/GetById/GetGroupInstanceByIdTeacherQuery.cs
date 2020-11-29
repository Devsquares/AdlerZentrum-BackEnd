using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.GroupInstance.Queries
{
    public class GetGroupInstanceByIdTeacherQuery : IRequest<Response<IEnumerable<TeacherGroupInstanceAssignment>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string TeacherId { get; set; }

        public class GetGroupInstanceByIdTeacherQueryHandler : IRequestHandler<GetGroupInstanceByIdTeacherQuery, Response<IEnumerable<TeacherGroupInstanceAssignment>>>
        {
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroup;
            public GetGroupInstanceByIdTeacherQueryHandler(ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroup)
            {
                _teacherGroup = teacherGroup;
            }
            public async Task<Response<IEnumerable<TeacherGroupInstanceAssignment>>> Handle(GetGroupInstanceByIdTeacherQuery query, CancellationToken cancellationToken)
            {
                var groupInstance = _teacherGroup.GetByTeacher(query.TeacherId);
                if (groupInstance == null) throw new ApiException($"Group Not Found.");
                return new Response<IEnumerable<TeacherGroupInstanceAssignment>>(groupInstance);
            }
        }
    }
}
