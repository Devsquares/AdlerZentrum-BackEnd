using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetLessonInstanceByIdQuery : IRequest<Response<LessonInstanceViewModel>>
    {
        public int Id { get; set; }
        public class GetLessonInstanceByIdQueryHandler : IRequestHandler<GetLessonInstanceByIdQuery, Response<LessonInstanceViewModel>>
        {
            private readonly ILessonInstanceRepositoryAsync _LessonInstanceRepository;
            private readonly IHomeWorkRepositoryAsync _homeworkRepository;
            private readonly IMapper _mapper;
            public GetLessonInstanceByIdQueryHandler(ILessonInstanceRepositoryAsync LessonInstanceRepository,
            IHomeWorkRepositoryAsync homeWorkRepositoryAsync,
             IMapper mapper)
            {
                _LessonInstanceRepository = LessonInstanceRepository;
                _homeworkRepository = homeWorkRepositoryAsync;
                _mapper = mapper;
            }
            public async Task<Response<LessonInstanceViewModel>> Handle(GetLessonInstanceByIdQuery query, CancellationToken cancellationToken)
            {
                var LessonInstance = await _LessonInstanceRepository.GetByIdAsync(query.Id);
                if (LessonInstance == null) throw new ApiException($"Group Not Found.");
                var LessonInstanceViewModel = _mapper.Map<LessonInstanceViewModel>(LessonInstance);
                LessonInstanceViewModel.Homework = _homeworkRepository.GetByLessonInstance(LessonInstance.Id);
                return new Response<LessonInstanceViewModel>(LessonInstanceViewModel);
            }
        }
    }
}
