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
    public class GetGroupInstanceByIdTeacherQuery : IRequest<PagedResponse<IEnumerable<TeacherGroupInstanceViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string TeacherId { get; set; }
        public List<int> Status { get; set; }

        public class GetGroupInstanceByIdTeacherQueryHandler : IRequestHandler<GetGroupInstanceByIdTeacherQuery, PagedResponse<IEnumerable<TeacherGroupInstanceViewModel>>>
        {
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroup;
            private readonly IMapper _mapper;
            public GetGroupInstanceByIdTeacherQueryHandler(ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroup, IMapper mapper)
            {
                _teacherGroup = teacherGroup;
                _mapper = mapper;
            }
            public async Task<PagedResponse<IEnumerable<TeacherGroupInstanceViewModel>>> Handle(GetGroupInstanceByIdTeacherQuery query, CancellationToken cancellationToken)
            {
                int count = 0;
                var groupInstance = _teacherGroup.GetByTeacher(query.TeacherId, query.Status, query.PageNumber, query.PageSize, out count);
                if (groupInstance == null) throw new ApiException($"Group Not Found.");

                var viewmodel = _mapper.Map<IReadOnlyList<TeacherGroupInstanceViewModel>>(groupInstance);
                return new PagedResponse<IEnumerable<TeacherGroupInstanceViewModel>>(viewmodel, query.PageNumber, query.PageSize, count);
            }
        }
    }
}
