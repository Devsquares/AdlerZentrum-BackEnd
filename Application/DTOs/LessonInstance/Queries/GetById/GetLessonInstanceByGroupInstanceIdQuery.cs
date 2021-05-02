using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetLessonInstanceByGroupInstanceIdQuery : IRequest<Response<IEnumerable<LessonInstanceViewModel>>>
    {
        public int GroupInstanceId { get; set; }
        public string TeacherId { get; set; }

        public class GetLessonInstanceByGroupInstanceIdQueryHandler : IRequestHandler<GetLessonInstanceByGroupInstanceIdQuery, Response<IEnumerable<LessonInstanceViewModel>>>
        {
            private readonly ILessonInstanceRepositoryAsync _lessonInstanceRepository;
            private readonly ITeacherGroupInstanceAssignmentRepositoryAsync _teacherGroupInstanceAssignmentRepository;
            private readonly IMapper _mapper;
            public GetLessonInstanceByGroupInstanceIdQueryHandler(ILessonInstanceRepositoryAsync lessonInstanceRepositoryAsync, IMapper mapper,
                ITeacherGroupInstanceAssignmentRepositoryAsync teacherGroupInstanceAssignmentRepository)
            {
                _lessonInstanceRepository = lessonInstanceRepositoryAsync;
                _teacherGroupInstanceAssignmentRepository = teacherGroupInstanceAssignmentRepository;
                _mapper = mapper;
            }

            public async Task<Response<IEnumerable<LessonInstanceViewModel>>> Handle(GetLessonInstanceByGroupInstanceIdQuery query, CancellationToken cancellationToken)
            {
                IEnumerable<LessonInstance> lessonInstances = new List<LessonInstance>();
                var teacherGroup = _teacherGroupInstanceAssignmentRepository.GetByTeachGroupInstanceId(query.TeacherId, query.GroupInstanceId);
                lessonInstances = _lessonInstanceRepository.GetByGroupInstanceId(query.GroupInstanceId);
                if (teacherGroup != null && !teacherGroup.IsDefault)
                {
                    if (!teacherGroup.LessonInstanceId.HasValue)
                    {
                        return new Response<IEnumerable<LessonInstanceViewModel>>($"Not the defualt teacher and doesn't have lesson assgined to him.");
                    }
                    lessonInstances = lessonInstances.Where(x => x.Id <= teacherGroup.LessonInstanceId);
                }

                if (lessonInstances == null) return new Response<IEnumerable<LessonInstanceViewModel>>($"Group Not Found.");
                var groupInstanceViewModel = _mapper.Map<IEnumerable<LessonInstanceViewModel>>(lessonInstances);
                return new Response<IEnumerable<LessonInstanceViewModel>>(groupInstanceViewModel);
            }
        }
    }
}
