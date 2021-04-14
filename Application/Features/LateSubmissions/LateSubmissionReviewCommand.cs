using Application.Exceptions;
using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class LateSubmissionReviewCommand : IRequest<Response<bool>>
    {
        public int SubmissionType { get; set; }
        public int Id { get; set; }
    }
    public class LateSubmissionReviewCommandHandler : IRequestHandler<LateSubmissionReviewCommand, Response<bool>>
    {
        private readonly IHomeWorkSubmitionRepositoryAsync _homeWorkSubmitionRepository;
        private readonly ITestInstanceRepositoryAsync _testInstanceRepository;
        private readonly ILessonInstanceRepositoryAsync _lessonInstanceRepository;
        private readonly IMapper _mapper;
        public LateSubmissionReviewCommandHandler(IHomeWorkSubmitionRepositoryAsync homeWorkSubmitionRepositoryAsync,
        ITestInstanceRepositoryAsync testInstanceRepositoryAsync,
        ILessonInstanceRepositoryAsync lessonInstanceRepositoryAsync,
         IMapper mapper)
        {
            _homeWorkSubmitionRepository = homeWorkSubmitionRepositoryAsync;
            _testInstanceRepository = testInstanceRepositoryAsync;
            _lessonInstanceRepository = lessonInstanceRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<bool>> Handle(LateSubmissionReviewCommand request, CancellationToken cancellationToken)
        {
            switch (request.SubmissionType)
            {
                case 1:
                    var lesson = await _lessonInstanceRepository.GetByIdAsync(request.Id);
                    if(lesson == null)
                    {
                        throw new ApiException("lesson not found");
                    }
                    lesson.DelaySeen = true;
                    await _lessonInstanceRepository.UpdateAsync(lesson);
                    break;
                case 2:
                    var homework = await _homeWorkSubmitionRepository.GetByIdAsync(request.Id);
                    if (homework == null)
                    {
                        throw new ApiException("homework not found");
                    }
                    homework.DelaySeen = true;
                    await _homeWorkSubmitionRepository.UpdateAsync(homework);
                    break;
                case 3:
                    var test = await _testInstanceRepository.GetByIdAsync(request.Id);
                    if (test == null)
                    {
                        throw new ApiException("test not found");
                    }
                    test.DelaySeen = true;
                    await _testInstanceRepository.UpdateAsync(test);
                    break;

                default:
                    break;
            }
            return new Response<bool>(true);
        }
    }
}
