using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.Level.Commands
{
    public class DeletePromoCodeByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeletePromoCodeByIdCommandHandler : IRequestHandler<DeletePromoCodeByIdCommand, Response<int>>
        {
            private readonly IPromoCodeRepositoryAsync _promoCodeRepository;
            public DeletePromoCodeByIdCommandHandler(IPromoCodeRepositoryAsync promoCodeRepository)
            {
                _promoCodeRepository = promoCodeRepository;
            }
            public async Task<Response<int>> Handle(DeletePromoCodeByIdCommand command, CancellationToken cancellationToken)
            {
                var Level = await _promoCodeRepository.GetByIdAsync(command.Id);
                if (Level == null) throw new ApiException($"PromoCode Not Found.");
                await _promoCodeRepository.DeleteAsync(Level);
                return new Response<int>(Level.Id);
            }
        }
    }
}
