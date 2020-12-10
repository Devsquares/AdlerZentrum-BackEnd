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

namespace Application.DTOs
{
    public class DeletePricingByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeletePricingByIdCommandHandler : IRequestHandler<DeletePricingByIdCommand, Response<int>>
        {
            private readonly IPricingRepositoryAsync _PricingRepositoryAsync;
            public DeletePricingByIdCommandHandler(IPricingRepositoryAsync PricingRepositoryAsync)
            {
                _PricingRepositoryAsync = PricingRepositoryAsync;
            }
            public async Task<Response<int>> Handle(DeletePricingByIdCommand command, CancellationToken cancellationToken)
            {
                var Pricing = await _PricingRepositoryAsync.GetByIdAsync(command.Id);
                if (Pricing == null) throw new ApiException($"Pricing Not Found.");
                await _PricingRepositoryAsync.DeleteAsync(Pricing);
                return new Response<int>(Pricing.Id);
            }
        }
    }
}
