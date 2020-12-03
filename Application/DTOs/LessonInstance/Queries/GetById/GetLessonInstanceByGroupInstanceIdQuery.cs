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

namespace Application.DTOs
{
    public class GetLessonInstanceByGroupInstanceIdQuery : IRequest<Response<IEnumerable<LessonInstance>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TeacherId { get; set; }

        public class GetLessonInstanceByGroupInstanceIdQueryHandler : IRequestHandler<GetLessonInstanceByGroupInstanceIdQuery, Response<IEnumerable<LessonInstance>>>
        {
            private readonly ILessonInstanceRepositoryAsync _lessonInstanceRepository;
            public GetLessonInstanceByGroupInstanceIdQueryHandler(ILessonInstanceRepositoryAsync lessonInstanceRepositoryAsync)
            {
                _lessonInstanceRepository = lessonInstanceRepositoryAsync;
            }
            public async Task<Response<IEnumerable<LessonInstance>>> Handle(GetLessonInstanceByGroupInstanceIdQuery query, CancellationToken cancellationToken)
            {
                var groupInstance = _lessonInstanceRepository.GetByGroupInstanceId(query.TeacherId);
                if (groupInstance == null) throw new ApiException($"Group Not Found.");
                return new Response<IEnumerable<LessonInstance>>(groupInstance);
            }
        }
    }
}
