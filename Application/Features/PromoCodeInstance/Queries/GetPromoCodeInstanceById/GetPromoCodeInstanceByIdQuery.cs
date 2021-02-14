using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PromoCodeInstance.Queries.GetPromoCodeInstanceById
{
    public class GetPromoCodeInstanceByIdQuery : IRequest<Response<Domain.Entities.PromoCodeInstance>>
    {
        public int Id { get; set; }
        public class GetPromoCodeInstanceByIdQueryHandler : IRequestHandler<GetPromoCodeInstanceByIdQuery, Response<Domain.Entities.PromoCodeInstance>>
        {
            private readonly IPromoCodeInstanceRepositoryAsync _promocodeinstanceRepository;
            public GetPromoCodeInstanceByIdQueryHandler(IPromoCodeInstanceRepositoryAsync promocodeinstanceRepository)
            {
                _promocodeinstanceRepository = promocodeinstanceRepository;
            }
            public async Task<Response<Domain.Entities.PromoCodeInstance>> Handle(GetPromoCodeInstanceByIdQuery query, CancellationToken cancellationToken)
            {
                var promocodeinstance = await _promocodeinstanceRepository.GetByIdAsync(query.Id);
                if (promocodeinstance == null) throw new ApiException($"PromoCodeInstance Not Found.");
                return new Response<Domain.Entities.PromoCodeInstance>(promocodeinstance);
            }
        }
    }
}
