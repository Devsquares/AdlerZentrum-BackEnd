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
    public class GetAllQuestionsQuery : IRequest<IEnumerable<GetAllQuestionsViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, IEnumerable<GetAllQuestionsViewModel>>
    {
        private readonly IQuestionRepositoryAsync _QuestionService;
        private readonly IMapper _mapper;
        public GetAllQuestionsQueryHandler(IQuestionRepositoryAsync QuestionService, IMapper mapper)
        {
            _QuestionService = QuestionService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllQuestionsViewModel>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<RequestParameter>(request);
            var user = await _QuestionService.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var userViewModel = _mapper.Map<IEnumerable<GetAllQuestionsViewModel>>(user);
            return userViewModel;
        }
    }
}
