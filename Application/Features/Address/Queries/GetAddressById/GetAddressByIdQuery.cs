using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Address.Queries.GetAddressById
{
    public class GetAddressByIdQuery : IRequest<Response<Domain.Entities.Address>>
    {
        public int Id { get; set; }
        public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, Response<Domain.Entities.Address>>
        {
            private readonly IAddressRepositoryAsync _addressRepository;
            public GetAddressByIdQueryHandler(IAddressRepositoryAsync addressRepository)
            {
                _addressRepository = addressRepository;
            }
            public async Task<Response<Domain.Entities.Address>> Handle(GetAddressByIdQuery query, CancellationToken cancellationToken)
            {
                var address = await _addressRepository.GetByIdAsync(query.Id);
                if (address == null) throw new ApiException($"Address Not Found.");
                return new Response<Domain.Entities.Address>(address);
            }
        }
    }
}
