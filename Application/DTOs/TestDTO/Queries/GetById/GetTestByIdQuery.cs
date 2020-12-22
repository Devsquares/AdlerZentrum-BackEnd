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
    public class GetTestByIdQuery : IRequest<Response<Domain.Entities.Test>>
    {
        public int Id { get; set; }
        public class GetTestByIdQueryHandler : IRequestHandler<GetTestByIdQuery, Response<Domain.Entities.Test>>
        {
            private readonly ITestRepositoryAsync _TestService;
            public GetTestByIdQueryHandler(ITestRepositoryAsync TestService)
            {
                _TestService = TestService;
            }
            public async Task<Response<Domain.Entities.Test>> Handle(GetTestByIdQuery query, CancellationToken cancellationToken)
            {
                var Test = await _TestService.GetByIdAsync(query.Id);
                if (Test == null) throw new ApiException($"Test Not Found.");
                return new Response<Domain.Entities.Test>(Test);
            }
        }
    }
}
