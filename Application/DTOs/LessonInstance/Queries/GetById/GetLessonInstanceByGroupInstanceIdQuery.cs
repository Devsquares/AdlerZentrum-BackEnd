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
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int GroupInstanceId { get; set; }

        public class GetLessonInstanceByGroupInstanceIdQueryHandler : IRequestHandler<GetLessonInstanceByGroupInstanceIdQuery, Response<IEnumerable<LessonInstanceViewModel>>>
        {
            private readonly ILessonInstanceRepositoryAsync _lessonInstanceRepository;
            private readonly IMapper _mapper;
            public GetLessonInstanceByGroupInstanceIdQueryHandler(ILessonInstanceRepositoryAsync lessonInstanceRepositoryAsync, IMapper mapper)
            {
                _lessonInstanceRepository = lessonInstanceRepositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<IEnumerable<LessonInstanceViewModel>>> Handle(GetLessonInstanceByGroupInstanceIdQuery query, CancellationToken cancellationToken)
            {
                var groupInstance = _lessonInstanceRepository.GetByGroupInstanceId(query.GroupInstanceId);
                if (groupInstance == null) throw new ApiException($"Group Not Found.");
                var groupInstanceViewModel = _mapper.Map<IEnumerable<LessonInstanceViewModel>>(groupInstance);
                return new Response<IEnumerable<LessonInstanceViewModel>>(groupInstanceViewModel);
            }
        }
    }
}
