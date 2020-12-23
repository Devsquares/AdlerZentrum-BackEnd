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
    public class GetLevelByIdQuery : IRequest<Response<Domain.Entities.Level>>
    {
        public int Id { get; set; }
        public class GetLevelByIdQueryHandler : IRequestHandler<GetLevelByIdQuery, Response<Domain.Entities.Level>>
        {
            private readonly ILevelRepositoryAsync _levelService;
            public GetLevelByIdQueryHandler(ILevelRepositoryAsync levelService)
            {
                _levelService = levelService;
            }
            public async Task<Response<Domain.Entities.Level>> Handle(GetLevelByIdQuery query, CancellationToken cancellationToken)
            {
                var level = await _levelService.GetByIdAsync(query.Id);
                if (level == null) throw new ApiException($"Level Not Found.");
                return new Response<Domain.Entities.Level>(level);
            }
        }
    }
}
