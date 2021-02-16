using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreatePromoCodeCommand : IRequest<Response<int>>
    {
        public string Name { get; set; } 

        public class CreatePromoCodeCommandHandler : IRequestHandler<CreatePromoCodeCommand, Response<int>>
        {
            private readonly IPromoCodeRepositoryAsync _promoCodeRepository;
            public CreatePromoCodeCommandHandler(IPromoCodeRepositoryAsync promoCodeRepository)
            {
                _promoCodeRepository = promoCodeRepository;
            }
            public async Task<Response<int>> Handle(CreatePromoCodeCommand command, CancellationToken cancellationToken)
            {
                var promo = new Domain.Entities.PromoCode();

                Reflection.CopyProperties(command, promo);
                promo.Status = 0;
                await _promoCodeRepository.AddAsync(promo);
                return new Response<int>(promo.Id);

            }
        }
    }
}
