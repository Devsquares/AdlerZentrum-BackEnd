using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Address.Commands.UpdateAddress
{
    public class UpdateAddressCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }

        public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Response<int>>
        {
            private readonly IAddressRepositoryAsync _addressRepository;
            public UpdateAddressCommandHandler(IAddressRepositoryAsync addressRepository)
            {
                _addressRepository = addressRepository;
            }
            public async Task<Response<int>> Handle(UpdateAddressCommand command, CancellationToken cancellationToken)
            {
                var address = await _addressRepository.GetByIdAsync(command.Id);

                if (address == null)
                {
                    throw new ApiException($"Address Not Found.");
                }
                else
                {
                    address.FullName = command.FullName;
                    address.Address1 = command.Address1;
                    address.Address2 = command.Address2;
                    address.PostCode = command.PostCode;
                    address.City = command.City;
                    address.Country = command.Country;
                    address.Phone = command.Phone;

                    await _addressRepository.UpdateAsync(address);
                    return new Response<int>(address.Id);
                }
            }
        }
    }
}
