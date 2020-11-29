using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.Level.Queries
{
    public class GetPromoCodeByIdQuery : IRequest<Response<Domain.Entities.PromoCode>>
    {
        public int Id { get; set; }
        public class GetLevelByIdQueryHandler : IRequestHandler<GetPromoCodeByIdQuery, Response<Domain.Entities.PromoCode>>
        {
            private readonly IPromoCodeRepositoryAsync _promoCodeRepository;
            public GetLevelByIdQueryHandler(IPromoCodeRepositoryAsync promoCodeRepository)
            {
                _promoCodeRepository = promoCodeRepository;
            }
            public async Task<Response<Domain.Entities.PromoCode>> Handle(GetPromoCodeByIdQuery query, CancellationToken cancellationToken)
            { 
                var promoCode = await _promoCodeRepository.GetByIdAsync(query.Id);
                if (promoCode == null) throw new ApiException($"Promo Code Not Found.");
                return new Response<Domain.Entities.PromoCode>(promoCode);
            }
        }
    }
}
