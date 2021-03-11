using Application.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Claims.Queries
{
    public class GetAllClaimsQuery : IRequest<IEnumerable<object>>
    {
        public class GetAllClaimsQueryHandler : IRequestHandler<GetAllClaimsQuery, IEnumerable<object>>
        {
            public GetAllClaimsQueryHandler()
            {
            }
            public async Task<IEnumerable<object>> Handle(GetAllClaimsQuery request, CancellationToken cancellationToken)
            {
                var claims = Enum.GetNames(typeof(ClaimsEnum)).Select(x=> new { Name = x}).ToList();
                return claims;
            }
        }
    }
}
