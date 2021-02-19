using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllQuestionsQuery : IRequest<PagedResponse<IEnumerable<GetAllQuestionsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int ? QuestionTypeId { get; set; }
    }
    public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, PagedResponse<IEnumerable<GetAllQuestionsViewModel>>>
    {
        private readonly IQuestionRepositoryAsync _QuestionService;
        private readonly IMapper _mapper;
        public GetAllQuestionsQueryHandler(IQuestionRepositoryAsync QuestionService, IMapper mapper)
        {
            _QuestionService = QuestionService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllQuestionsViewModel>>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            var user = await _QuestionService.GetAllAsync(validFilter.PageNumber, validFilter.PageSize, request.QuestionTypeId);
            var userViewModel = _mapper.Map<IEnumerable<GetAllQuestionsViewModel>>(user);
            return new PagedResponse<IEnumerable<GetAllQuestionsViewModel>>(userViewModel, validFilter.PageNumber, validFilter.PageSize, _QuestionService.GetCount());
        }
    }
}
