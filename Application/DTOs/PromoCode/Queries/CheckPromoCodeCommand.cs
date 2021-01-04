using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CheckPromoCodeCommand : IRequest<Response<bool>>
    {
        public string name { get; set; }
        public class CheckPromoCodeCommandHandler : IRequestHandler<CheckPromoCodeCommand, Response<bool>>
        {
            private readonly IPromoCodeRepositoryAsync _promoCodeRepository;
            public CheckPromoCodeCommandHandler(IPromoCodeRepositoryAsync promoCodeRepository)
            {
                _promoCodeRepository = promoCodeRepository;
            }
            public async Task<Response<bool>> Handle(CheckPromoCodeCommand query, CancellationToken cancellationToken)
            {
                var promoCode = _promoCodeRepository.CheckPromoCode(query.name);
                if (promoCode == null) throw new ApiException($"Promo Code Not Found.");
                return new Response<bool>(promoCode);
            }
        }
    }
}
