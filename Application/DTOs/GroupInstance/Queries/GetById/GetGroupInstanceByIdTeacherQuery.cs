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
using AutoMapper;

namespace Application.DTOs.GroupInstance.Queries
{
    public class GetGroupInstanceByIdTeacherQuery : IRequest<Response<IEnumerable<TeacherGroupInstanceViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string TeacherId { get; set; }

        public class GetGroupInstanceByIdTeacherQueryHandler : IRequestHandler<GetGroupInstanceByIdTeacherQuery, Response<IEnumerable<TeacherGroupInstanceViewModel>>>
        {
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroup;
            private readonly IMapper _mapper;
            public GetGroupInstanceByIdTeacherQueryHandler(ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroup, IMapper mapper)
            {
                _teacherGroup = teacherGroup;
                _mapper = mapper;
            }
            public async Task<Response<IEnumerable<TeacherGroupInstanceViewModel>>> Handle(GetGroupInstanceByIdTeacherQuery query, CancellationToken cancellationToken)
            {
                var groupInstance = _teacherGroup.GetByTeacher(query.TeacherId);
                if (groupInstance == null) throw new ApiException($"Group Not Found.");

                var viewmodel = _mapper.Map<IReadOnlyList<TeacherGroupInstanceViewModel>>(groupInstance);
                return new Response<IEnumerable<TeacherGroupInstanceViewModel>>(viewmodel);
            }
        }
    }
}
