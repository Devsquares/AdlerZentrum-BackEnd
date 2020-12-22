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
    public class GetSingleQuestionByIdQuery : IRequest<Response<Domain.Entities.SingleQuestion>>
    {
        public int Id { get; set; }
        public class GetSingleQuestionByIdQueryHandler : IRequestHandler<GetSingleQuestionByIdQuery, Response<Domain.Entities.SingleQuestion>>
        {
            private readonly ISingleQuestionRepositoryAsync _SingleQuestionService;
            public GetSingleQuestionByIdQueryHandler(ISingleQuestionRepositoryAsync SingleQuestionService)
            {
                _SingleQuestionService = SingleQuestionService;
            }
            public async Task<Response<Domain.Entities.SingleQuestion>> Handle(GetSingleQuestionByIdQuery query, CancellationToken cancellationToken)
            {
                var SingleQuestion = await _SingleQuestionService.GetByIdAsync(query.Id);
                if (SingleQuestion == null) throw new ApiException($"SingleQuestion Not Found.");
                return new Response<Domain.Entities.SingleQuestion>(SingleQuestion);
            }
        }
    }
}
