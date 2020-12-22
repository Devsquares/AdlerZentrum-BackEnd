using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetStudentsByLessonInstanceQuery : IRequest<Response<IEnumerable<StudentsByLessonViewModel>>>
    {
        public int LessonInstanceId { get; set; }
        public class GetStudentsByLessonInstanceQueryHandler : IRequestHandler<GetStudentsByLessonInstanceQuery, Response<IEnumerable<StudentsByLessonViewModel>>>
        {
            private readonly ILessonInstanceStudentRepositoryAsync _lessonInstanceRepository;
            private readonly IMapper _mapper;
            public GetStudentsByLessonInstanceQueryHandler(ILessonInstanceStudentRepositoryAsync lessonInstanceRepositoryAsync, IMapper mapper)
            {
                _lessonInstanceRepository = lessonInstanceRepositoryAsync;
                _mapper = mapper;
            }
            public async Task<Response<IEnumerable<StudentsByLessonViewModel>>> Handle(GetStudentsByLessonInstanceQuery query, CancellationToken cancellationToken)
            {
                var groupInstance = _lessonInstanceRepository.GetStudentsByLessonInstance(query.LessonInstanceId);
                if (groupInstance == null) throw new ApiException($"Lesson Not Found.");
                var groupInstanceViewModel = _mapper.Map<IEnumerable<StudentsByLessonViewModel>>(groupInstance);
                return new Response<IEnumerable<StudentsByLessonViewModel>>(groupInstanceViewModel);
            }
        }
    }
}
