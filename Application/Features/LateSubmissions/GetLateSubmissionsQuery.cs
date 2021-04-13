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
    public class GetLateSubmissionsQuery : IRequest<PagedResponse<List<LateSubmissionsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int SubmissionType { get; set; }
        public string TeacherName { get; set; }
        public bool DelaySeen { get; set; } = false;
    }
    public class GetLateSubmissionsQueryHandler : IRequestHandler<GetLateSubmissionsQuery, PagedResponse<List<LateSubmissionsViewModel>>>
    {
        private readonly IHomeWorkSubmitionRepositoryAsync _homeWorkSubmitionRepository;
        private readonly ITestInstanceRepositoryAsync _testInstanceRepository;
        private readonly ILessonInstanceRepositoryAsync _lessonInstanceRepository;
        private readonly IMapper _mapper;
        public GetLateSubmissionsQueryHandler(IHomeWorkSubmitionRepositoryAsync homeWorkSubmitionRepositoryAsync,
        ITestInstanceRepositoryAsync testInstanceRepositoryAsync,
        ILessonInstanceRepositoryAsync lessonInstanceRepositoryAsync,
         IMapper mapper)
        {
            _homeWorkSubmitionRepository = homeWorkSubmitionRepositoryAsync;
            _testInstanceRepository = testInstanceRepositoryAsync;
            _lessonInstanceRepository = lessonInstanceRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<LateSubmissionsViewModel>>> Handle(GetLateSubmissionsQuery request, CancellationToken cancellationToken)
        {
            int count = 0;
            if (request.PageSize <= 0) request.PageSize = 10;
            if (request.PageNumber <= 0) request.PageNumber = 1;

            List<LateSubmissionsViewModel> output = new List<LateSubmissionsViewModel>();
            switch (request.SubmissionType)
            {
                case 1:
                    output = await _lessonInstanceRepository.GetLateSubmissions(request.TeacherName, request.PageNumber, request.PageSize,request.DelaySeen);
                    count = _lessonInstanceRepository.GetLateSubmissionsCount(request.TeacherName,request.DelaySeen);
                    break;
                case 2:
                    output = await _homeWorkSubmitionRepository.GetLateSubmissions(request.TeacherName, request.PageNumber, request.PageSize,request.DelaySeen);
                    count = _homeWorkSubmitionRepository.GetLateSubmissionsCount(request.TeacherName,request.DelaySeen);
                    break;
                case 3:
                    output = await _testInstanceRepository.GetLateSubmissions(request.TeacherName, request.PageNumber, request.PageSize,request.DelaySeen);
                    count = _testInstanceRepository.GetLateSubmissionsCount(request.TeacherName,request.DelaySeen);
                    break;

                default:
                    break;
            }
            return new Wrappers.PagedResponse<List<LateSubmissionsViewModel>>(output, request.PageNumber, request.PageSize, count);
        }
    }
}
