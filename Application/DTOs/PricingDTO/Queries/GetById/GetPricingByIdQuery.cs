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
    public class GetPricingByIdQuery : IRequest<Response<Domain.Entities.Pricing>>
    {
        public int Id { get; set; }
        public class GetPricingByIdQueryHandler : IRequestHandler<GetPricingByIdQuery, Response<Domain.Entities.Pricing>>
        {
            private readonly IPricingRepositoryAsync _PricingService;
            public GetPricingByIdQueryHandler(IPricingRepositoryAsync PricingService)
            {
                _PricingService = PricingService;
            }
            public async Task<Response<Domain.Entities.Pricing>> Handle(GetPricingByIdQuery query, CancellationToken cancellationToken)
            {
                var Pricing = await _PricingService.GetByIdAsync(query.Id);
                if (Pricing == null) throw new ApiException($"Pricing Not Found.");
                return new Response<Domain.Entities.Pricing>(Pricing);
            }
        }
    }
}
