using Application.Enums;
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
    public class GetAllFeedbackSheetsQuery : IRequest<FeedbackSheetPagedResponse<IEnumerable<GetAllFeedbackSheetsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int GroupInstanceId { get; set; }
        public string StudentName { get; set; }
        public TestInstanceEnum Status { get; set; }
        public int LessonInstanceId { get; set; }
    }
    public class GetAllFeedbackSheetsQueryHandler : IRequestHandler<GetAllFeedbackSheetsQuery, FeedbackSheetPagedResponse<IEnumerable<GetAllFeedbackSheetsViewModel>>>
    {
        private readonly ITestInstanceRepositoryAsync _testInstanceRepository;
        private readonly IMapper _mapper;
        public GetAllFeedbackSheetsQueryHandler(ITestInstanceRepositoryAsync testInstanceRepositoryAsync, IMapper mapper)
        {
            _testInstanceRepository = testInstanceRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<FeedbackSheetPagedResponse<IEnumerable<GetAllFeedbackSheetsViewModel>>> Handle(GetAllFeedbackSheetsQuery request, CancellationToken cancellationToken)
        {
            int count = await _testInstanceRepository.GetFeedbackSheetsPagedReponseCountAsync(request.GroupInstanceId, request.StudentName, request.Status, request.LessonInstanceId);
            var forumtopic = await _testInstanceRepository.GetFeedbackSheetsPagedReponseAsync(request.PageNumber, request.PageSize, request.GroupInstanceId, request.StudentName, request.Status, request.LessonInstanceId);
            var forumtopicViewModel = _mapper.Map<IEnumerable<GetAllFeedbackSheetsViewModel>>(forumtopic);
            return new Wrappers.FeedbackSheetPagedResponse<IEnumerable<GetAllFeedbackSheetsViewModel>>(forumtopicViewModel, request.PageNumber, request.PageSize, count, request.GroupInstanceId, request.StudentName, request.Status, request.LessonInstanceId);
        }
    }
}
