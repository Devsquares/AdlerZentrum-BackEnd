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
    public class GetDisqualificationRequestByIdQuery : IRequest<Response<Domain.Entities.DisqualificationRequest>>
    {
        public int Id { get; set; }
        public class GetDisqualificationRequestByIdQueryHandler : IRequestHandler<GetDisqualificationRequestByIdQuery, Response<Domain.Entities.DisqualificationRequest>>
        {
            private readonly IDisqualificationRequestRepositoryAsync _disqualificationrequestRepository;
            public GetDisqualificationRequestByIdQueryHandler(IDisqualificationRequestRepositoryAsync disqualificationrequestRepository)
            {
                _disqualificationrequestRepository = disqualificationrequestRepository;
            }
            public async Task<Response<Domain.Entities.DisqualificationRequest>> Handle(GetDisqualificationRequestByIdQuery query, CancellationToken cancellationToken)
            {
                var disqualificationrequest = await _disqualificationrequestRepository.GetByIdAsync(query.Id);
                if (disqualificationrequest == null) throw new ApiException($"DisqualificationRequest Not Found.");
                return new Response<Domain.Entities.DisqualificationRequest>(disqualificationrequest);
            }
        }
    }
}
