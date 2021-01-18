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
    public class GetAllSingleQuestionsQuery : IRequest<PagedResponse<IEnumerable<GetAllSingleQuestionsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllSingleQuestionsQueryHandler : IRequestHandler<GetAllSingleQuestionsQuery, PagedResponse<IEnumerable<GetAllSingleQuestionsViewModel>>>
    {
        private readonly ISingleQuestionRepositoryAsync _SingleQuestionService;
        private readonly IMapper _mapper;
        public GetAllSingleQuestionsQueryHandler(ISingleQuestionRepositoryAsync SingleQuestionService, IMapper mapper)
        {
            _SingleQuestionService = SingleQuestionService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllSingleQuestionsViewModel>>> Handle(GetAllSingleQuestionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            var user = await _SingleQuestionService.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize, "Choices");
            var userViewModel = _mapper.Map<IEnumerable<GetAllSingleQuestionsViewModel>>(user);
            return new PagedResponse<IEnumerable<GetAllSingleQuestionsViewModel>>(userViewModel, validFilter.PageNumber, validFilter.PageSize,_SingleQuestionService.GetCount());
        }
    }
}
