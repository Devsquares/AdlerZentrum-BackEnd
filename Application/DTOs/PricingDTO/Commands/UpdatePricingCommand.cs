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

namespace Application.DTOs.Pricing.Commands
{
    public class UpdatePricingCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int? Status { get; set; }

        public class UpdatePricingCommandHandler : IRequestHandler<UpdatePricingCommand, Response<int>>
        {
            private readonly IPricingRepositoryAsync _PricingRepository;
            public UpdatePricingCommandHandler(IPricingRepositoryAsync PricingRepository)
            {
                _PricingRepository = PricingRepository;
            }
            public async Task<Response<int>> Handle(UpdatePricingCommand command, CancellationToken cancellationToken)
            {
                var Pricing = await _PricingRepository.GetByIdAsync(command.Id);

                if (Pricing == null)
                {
                    throw new ApiException($"Pricing Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, Pricing);
                    await _PricingRepository.UpdateAsync(Pricing);
                    return new Response<int>(Pricing.Id);
                }
            }
        }
    }
}
