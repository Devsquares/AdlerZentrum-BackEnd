using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
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
                var promoCode = await _promoCodeRepository.GetByIdAsync(command.Id);
                if (promoCode == null) throw new ApiException($"PromoCode Not Found.");
                await _promoCodeRepository.DeleteAsync(promoCode);
                return new Response<int>(promoCode.Id);
            }
        }
    }
}
