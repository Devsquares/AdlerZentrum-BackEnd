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
    public class GetSublevelByIdQuery : IRequest<Response<Domain.Entities.Sublevel>>
    {
        public int Id { get; set; }
        public class GetSublevelByIdQueryHandler : IRequestHandler<GetSublevelByIdQuery, Response<Domain.Entities.Sublevel>>
        {
            private readonly ISublevelRepositoryAsync _SublevelService;
            public GetSublevelByIdQueryHandler(ISublevelRepositoryAsync SublevelService)
            {
                _SublevelService = SublevelService;
            }
            public async Task<Response<Domain.Entities.Sublevel>> Handle(GetSublevelByIdQuery query, CancellationToken cancellationToken)
            {
                var Sublevel = await _SublevelService.GetByIdAsync(query.Id);
                if (Sublevel == null) throw new ApiException($"Sublevel Not Found.");
                return new Response<Domain.Entities.Sublevel>(Sublevel);
            }
        }
    }
}
