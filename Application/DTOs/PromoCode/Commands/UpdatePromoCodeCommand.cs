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

namespace Application.DTOs.Level.Commands
{
    public class UpdatePromoCodeCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; } 

        public class UpdatePromoCodeCommandHandler : IRequestHandler<UpdatePromoCodeCommand, Response<int>>
        {
            private readonly IPromoCodeRepositoryAsync _promoCodeRepository;
            public UpdatePromoCodeCommandHandler(IPromoCodeRepositoryAsync promoCodeRepository)
            {
                _promoCodeRepository = promoCodeRepository;
            }
            public async Task<Response<int>> Handle(UpdatePromoCodeCommand command, CancellationToken cancellationToken)
            {
                var level = await _promoCodeRepository.GetByIdAsync(command.Id);

                if (level == null)
                {
                    throw new ApiException($"PromoCode Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, level);
                    await _promoCodeRepository.UpdateAsync(level);
                    return new Response<int>(level.Id);
                }
            }
        }
    }
}
