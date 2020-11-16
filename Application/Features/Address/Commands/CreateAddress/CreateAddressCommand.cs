using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Address.Commands.CreateAddress
{
    public partial class CreateAddressCommand :  IRequest<Response<int>>
    {
        public string FullName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
    }

    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Response<int>>
    {
        private readonly IAddressRepositoryAsync _addressRepository;
        private readonly IMapper _mapper;
        public CreateAddressCommandHandler(IAddressRepositoryAsync addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var address = _mapper.Map<Domain.Entities.Address>(request);
            await _addressRepository.AddAsync(address);
            return new Response<int>(address.Id);
        }
    }
}
