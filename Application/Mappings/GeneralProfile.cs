using Application.DTOs.Account.Queries.GetAllUsers;
using Application.DTOs.GroupInstance.Queries;
using Application.DTOs.Level.Commands;
using Application.DTOs.Level.Queries;
using Application.Filters;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Level, GetAllLevelsViewModel>().ReverseMap();
            CreateMap<CreateLevelCommand, Level>();
            CreateMap<GetAllLevelsQuery, PagedResponse<Level>>();


            CreateMap<Account, GetAllUsersViewModel>().ReverseMap();
            CreateMap<GetAllUsersQuery, GetAllUsersParameter>();

            // Filter 
            CreateMap<GetAllLevelsQuery, RequestParameter>();
            CreateMap<GetAllUsersQuery, RequestParameter>();
            CreateMap<GetAllGroupInstancesQuery, RequestParameter>();

        }
    }
}
