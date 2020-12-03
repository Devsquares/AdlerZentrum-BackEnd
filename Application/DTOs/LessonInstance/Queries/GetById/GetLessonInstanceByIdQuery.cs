using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetLessonInstanceByIdQuery : IRequest<Response<Domain.Entities.LessonInstance>>
    {
        public int Id { get; set; }
        public class GetLessonInstanceByIdQueryHandler : IRequestHandler<GetLessonInstanceByIdQuery, Response<Domain.Entities.LessonInstance>>
        {
            private readonly ILessonInstanceRepositoryAsync _LessonInstanceRepository;
            public GetLessonInstanceByIdQueryHandler(ILessonInstanceRepositoryAsync LessonInstanceRepository)
            {
                _LessonInstanceRepository = LessonInstanceRepository;
            }
            public async Task<Response<Domain.Entities.LessonInstance>> Handle(GetLessonInstanceByIdQuery query, CancellationToken cancellationToken)
            {
                var LessonInstance = await _LessonInstanceRepository.GetByIdAsync(query.Id);
                if (LessonInstance == null) throw new ApiException($"Group Not Found.");
                return new Response<Domain.Entities.LessonInstance>(LessonInstance);
            }
        }
    }
}
