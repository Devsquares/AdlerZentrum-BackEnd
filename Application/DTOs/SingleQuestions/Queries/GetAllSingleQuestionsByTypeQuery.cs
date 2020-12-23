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
    public class GetAllSingleQuestionsByTypeQuery : IRequest<PagedResponse<IEnumerable<GetAllSingleQuestionsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TypeId { get; set; }
    }
    public class GetAllSingleQuestionsByTypeQueryHandler : IRequestHandler<GetAllSingleQuestionsByTypeQuery, PagedResponse<IEnumerable<GetAllSingleQuestionsViewModel>>>
    {
        private readonly ISingleQuestionRepositoryAsync _SingleQuestionService;
        private readonly IMapper _mapper;
        public GetAllSingleQuestionsByTypeQueryHandler(ISingleQuestionRepositoryAsync SingleQuestionService, IMapper mapper)
        {
            _SingleQuestionService = SingleQuestionService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllSingleQuestionsViewModel>>> Handle(GetAllSingleQuestionsByTypeQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber == 0) request.PageNumber = 1;
            if (request.PageSize == 0) request.PageSize = 10;
            var user = await _SingleQuestionService.GetPagedReponseAsync(request.PageNumber, request.PageSize, request.TypeId);
            var userViewModel = _mapper.Map<IEnumerable<GetAllSingleQuestionsViewModel>>(user);
            return new PagedResponse<IEnumerable<GetAllSingleQuestionsViewModel>>(userViewModel, request.PageNumber, request.PageSize);
        }
    }
}
