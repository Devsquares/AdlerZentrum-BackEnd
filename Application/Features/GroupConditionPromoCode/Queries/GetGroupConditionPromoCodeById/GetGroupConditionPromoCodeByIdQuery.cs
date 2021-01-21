using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.GroupConditionPromoCode.Queries.GetGroupConditionPromoCodeById
{
    public class GetGroupConditionPromoCodeByIdQuery : IRequest<Response<Domain.Entities.GroupConditionPromoCode>>
    {
        public int Id { get; set; }
        public class GetGroupConditionPromoCodeByIdQueryHandler : IRequestHandler<GetGroupConditionPromoCodeByIdQuery, Response<Domain.Entities.GroupConditionPromoCode>>
        {
            private readonly IGroupConditionPromoCodeRepositoryAsync _groupconditionpromocodeRepository;
            public GetGroupConditionPromoCodeByIdQueryHandler(IGroupConditionPromoCodeRepositoryAsync groupconditionpromocodeRepository)
            {
                _groupconditionpromocodeRepository = groupconditionpromocodeRepository;
            }
            public async Task<Response<Domain.Entities.GroupConditionPromoCode>> Handle(GetGroupConditionPromoCodeByIdQuery query, CancellationToken cancellationToken)
            {
                var groupconditionpromocode = await _groupconditionpromocodeRepository.GetByIdAsync(query.Id);
                if (groupconditionpromocode == null) throw new ApiException($"GroupConditionPromoCode Not Found.");
                return new Response<Domain.Entities.GroupConditionPromoCode>(groupconditionpromocode);
            }
        }
    }
}
