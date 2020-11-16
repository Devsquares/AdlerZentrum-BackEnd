using Application.DTOs.Account.Queries.GetAllUsers;
using Application.Features.Address.Commands.CreateAddress;
using Application.Features.Address.Queries.GetAllAddresses; 
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Address, GetAllAddressesViewModel>().ReverseMap();
            CreateMap<CreateAddressCommand, Address>();
            CreateMap<GetAllAddressesQuery, GetAllAddressesParameter>();
 
            CreateMap<Account, GetAllUsersViewModel>().ReverseMap();
            CreateMap<GetAllUsersQuery, GetAllUsersParameter>();

        }
    }
}
