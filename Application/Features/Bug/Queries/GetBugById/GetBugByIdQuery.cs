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
    public class GetBugByIdQuery : IRequest<Response<Domain.Entities.Bug>>
    {
        public int Id { get; set; }
        public class GetBugByIdQueryHandler : IRequestHandler<GetBugByIdQuery, Response<Domain.Entities.Bug>>
        {
            private readonly IBugRepositoryAsync _bugRepository;
            public GetBugByIdQueryHandler(IBugRepositoryAsync bugRepository)
            {
                _bugRepository = bugRepository;
            }
            public async Task<Response<Domain.Entities.Bug>> Handle(GetBugByIdQuery query, CancellationToken cancellationToken)
            {
                var bug = await _bugRepository.GetByIdAsync(query.Id);
                if (bug == null) throw new ApiException($"Bug Not Found.");
                return new Response<Domain.Entities.Bug>(bug);
            }
        }
    }
}
