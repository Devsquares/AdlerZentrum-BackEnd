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
    public class CreatePricingCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int? Status { get; set; }

        public class CreatePricingCommandHandler : IRequestHandler<CreatePricingCommand, Response<int>>
        {
            private readonly IPricingRepositoryAsync _PricingRepository;
            public CreatePricingCommandHandler(IPricingRepositoryAsync PricingRepository)
            {
                _PricingRepository = PricingRepository;
            }
            public async Task<Response<int>> Handle(CreatePricingCommand command, CancellationToken cancellationToken)
            {
                var Pricing = new Domain.Entities.Pricing();

                Reflection.CopyProperties(command, Pricing);
                await _PricingRepository.AddAsync(Pricing);
                return new Response<int>(Pricing.Id);

            }
        }
    }
}
