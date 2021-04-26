using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using MediatR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.GroupInstance.Queries
{
    public class GetAllTeacerGroupInstanceAssignmentsQuery : IRequest<PagedResponse<object>>
    {
        public int? GroupDefinitionId { get; set; }
        public int? SublevelId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class GetAllTeacerGroupInstanceAssignmentsQueryHandler : IRequestHandler<GetAllTeacerGroupInstanceAssignmentsQuery, PagedResponse<object>>
        {
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroupInstanceAssignment;
            private readonly IMapper _mapper;
            public GetAllTeacerGroupInstanceAssignmentsQueryHandler(ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignment, IMapper mapper)
            {
                _teacherGroupInstanceAssignment = teacherGroupInstanceAssignment;
                _mapper = mapper;
            }
            public async Task<PagedResponse<object>> Handle(GetAllTeacerGroupInstanceAssignmentsQuery command, CancellationToken cancellationToken)
            {
                int totalCount = 0;
                var groupInstance = _teacherGroupInstanceAssignment.GetAll(command.PageNumber, command.PageSize, out totalCount, command.SublevelId, command.GroupDefinitionId);
                var group = groupInstance.GroupBy(x => x.GroupInstanceId);
                //var viewmodel = _mapper.Map<List<GetAllTeacerGroupInstanceAssignmentViewModel>>(groupInstance);
                return new PagedResponse<object>(group, command.PageNumber, command.PageSize, totalCount);
            }
        }
    }
}
