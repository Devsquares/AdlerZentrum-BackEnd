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
    public class CheckPromoCodeCommand : IRequest<Response<int>>
    {
        public string name { get; set; }
        public class CheckPromoCodeCommandHandler : IRequestHandler<CheckPromoCodeCommand, Response<int>>
        {
            private readonly IPromoCodeRepositoryAsync _promoCodeRepository;
            public CheckPromoCodeCommandHandler(IPromoCodeRepositoryAsync promoCodeRepository)
            {
                _promoCodeRepository = promoCodeRepository;
            }
            public async Task<Response<int>> Handle(CheckPromoCodeCommand query, CancellationToken cancellationToken)
            {
                var promoCode = _promoCodeRepository.CheckPromoCode(query.name);
                if (promoCode == null) throw new ApiException($"Promo Code Not Found.");
                return new Response<int>(promoCode.Id);
            }
        }
    }
}
