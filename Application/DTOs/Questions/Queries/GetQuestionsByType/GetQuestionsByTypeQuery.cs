using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetQuestionsByTypeQuery : IRequest<PagedResponse<IEnumerable<Question>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int QuestionType { get; set; }
        public bool NotUsed { get; set; }
    }
    public class GetQuestionsByTypeQueryHandler : IRequestHandler<GetQuestionsByTypeQuery, PagedResponse<IEnumerable<Question>>>
    {
        private readonly IQuestionRepositoryAsync _QuestionService;
        private readonly IMapper _mapper;
        public GetQuestionsByTypeQueryHandler(IQuestionRepositoryAsync QuestionService, IMapper mapper)
        {
            _QuestionService = QuestionService;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<Question>>> Handle(GetQuestionsByTypeQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber <= 0)
            {
                request.PageNumber = 1;
            }
            if (request.PageSize <= 0)
            {
                request.PageSize = 10;
            }
            List<Question> questions = new List<Question>();
            int count = 0;
            if (request.NotUsed)
            {
                questions = _QuestionService.GetAllByTypeIdNotUsedAsync(request.QuestionType, request.PageNumber, request.PageSize).Result;
                count = _QuestionService.GetAllByTypeIdCountNotUsedAsync(request.QuestionType);
            }
            else
            {
                questions = _QuestionService.GetAllByTypeIdAsync(request.QuestionType, request.PageNumber, request.PageSize).Result;
                count = _QuestionService.GetAllByTypeIdCountAsync(request.QuestionType);
            }
            return new PagedResponse<IEnumerable<Question>>(questions, request.PageNumber, request.PageSize, count);
        }
    }
}
