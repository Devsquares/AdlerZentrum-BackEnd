using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetSublevelByIdQuery : IRequest<Response<Domain.Entities.Sublevel>>
    {
        public int Id { get; set; }
        public class GetSublevelByIdQueryHandler : IRequestHandler<GetSublevelByIdQuery, Response<Domain.Entities.Sublevel>>
        {
            private readonly ISublevelRepositoryAsync _sublevelRepository;
            public GetSublevelByIdQueryHandler(ISublevelRepositoryAsync sublevelRepository)
            {
                _sublevelRepository = sublevelRepository;
            }
            public async Task<Response<Domain.Entities.Sublevel>> Handle(GetSublevelByIdQuery query, CancellationToken cancellationToken)
            {
                var sublevel = await _sublevelRepository.GetByIdAsync(query.Id);
                if (sublevel == null) throw new ApiException($"Sublevel Not Found.");
                return new Response<Domain.Entities.Sublevel>(sublevel);
            }
        }
    }
}
