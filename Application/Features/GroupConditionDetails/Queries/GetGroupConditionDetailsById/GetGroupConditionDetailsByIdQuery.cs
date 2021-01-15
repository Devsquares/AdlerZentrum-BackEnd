using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupConditionDetails.Queries.GetGroupConditionDetailsById
{
    public class GetGroupConditionDetailsByIdQuery : IRequest<Response<Domain.Entities.GroupConditionDetail>>
    {
        public int Id { get; set; }
        public class GetGroupConditionDetailsByIdQueryHandler : IRequestHandler<GetGroupConditionDetailsByIdQuery, Response<Domain.Entities.GroupConditionDetail>>
        {
            private readonly IGroupConditionDetailsRepositoryAsync _groupconditiondetailsRepository;
            public GetGroupConditionDetailsByIdQueryHandler(IGroupConditionDetailsRepositoryAsync groupconditiondetailsRepository)
            {
                _groupconditiondetailsRepository = groupconditiondetailsRepository;
            }
            public async Task<Response<Domain.Entities.GroupConditionDetail>> Handle(GetGroupConditionDetailsByIdQuery query, CancellationToken cancellationToken)
            {
                var groupconditiondetails = await _groupconditiondetailsRepository.GetByIdAsync(query.Id);
                if (groupconditiondetails == null) throw new ApiException($"GroupConditionDetails Not Found.");
                return new Response<Domain.Entities.GroupConditionDetail>(groupconditiondetails);
            }
        }
    }
}
