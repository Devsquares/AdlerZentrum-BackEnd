using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetQuestionByIdQuery : IRequest<Response<Domain.Entities.Question>>
    {
        public int Id { get; set; }
        public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, Response<Domain.Entities.Question>>
        {
            private readonly IQuestionRepositoryAsync _QuestionService;
            public GetQuestionByIdQueryHandler(IQuestionRepositoryAsync QuestionService)
            {
                _QuestionService = QuestionService;
            }
            public async Task<Response<Domain.Entities.Question>> Handle(GetQuestionByIdQuery query, CancellationToken cancellationToken)
            {
                var Question = await _QuestionService.GetByIdAsync(query.Id);
                if (Question == null) throw new ApiException($"Question Not Found.");
                return new Response<Domain.Entities.Question>(Question);
            }
        }
    }
}
