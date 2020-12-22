using Application.DTOs.Account;
using Application.DTOs;
using Application.DTOs.Account.Queries.GetAllUsers;
using Application.DTOs.GroupCondition.Queries;
using Application.DTOs.GroupInstance.Queries;
using Application.DTOs.HomeWorkSubmitionDTO.Queries;
using Application.DTOs.Level.Commands;
using Application.DTOs.Level.Queries;
using Application.DTOs.Pricing.Queries;
using Application.DTOs.TimeSlot.Queries;
using Application.Enums;
using Application.Filters;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Level, GetAllLevelsViewModel>().ReverseMap();
            CreateMap<CreateLevelCommand, Level>();
            CreateMap<GetAllLevelsQuery, PagedResponse<Level>>();


            CreateMap<ApplicationUser, GetAllUsersViewModel>().ReverseMap();
            CreateMap<ApplicationUser, AccountViewModel>().ReverseMap();
            CreateMap<GetAllUsersQuery, GetAllUsersParameter>();

            CreateMap<GroupInstance, GetAllGroupInstancesViewModel>().ReverseMap();
            CreateMap<GroupCondition, GetAllGroupConditionViewModel>().ReverseMap();

            CreateMap<TimeSlot, GetAllTimeSlotsViewModel>().ReverseMap();
            CreateMap<Pricing, GetAllPricingViewModel>().ReverseMap();


            CreateMap<SingleQuestion, GetAllSingleQuestionsViewModel>().ReverseMap();
            CreateMap<Question, GetQuestionViewModel>().ReverseMap();

            //CreateMap<HomeWorkSubmition, GetAllHomeWorkSubmitionsViewModel>().ReverseMap();
            CreateMap<HomeWorkSubmition, GetAllHomeWorkSubmitionsViewModel>()
                             .ForMember(destination => destination.Status,
                 opt => opt.MapFrom(source => Enum.GetName(typeof(HomeWorkSubmitionStatusEnum), source.Status)));

            CreateMap<Homework, GetAllomeworkBounsViewModel>()
            .ForMember(destination => destination.GroupInstanceSerial, opts => opts.MapFrom(source => source.GroupInstance.Serial))
            .ForMember(destination => destination.TeacherName, opts => opts.MapFrom(source => source.Teacher.FirstName + source.Teacher.LastName))
            .ForMember(destination => destination.LessonOrder, opts => opts.MapFrom(source => source.LessonInstance.Serial))
            .ForMember(destination => destination.SubLevel, opts => opts.MapFrom(source => source.GroupInstance.GroupDefinition.Sublevel.Name))
            .ForMember(destination => destination.LessonInstanceSerial, opts => opts.MapFrom(source => source.LessonInstance.Serial));

            CreateMap<LessonInstance, LessonInstanceViewModel>().ReverseMap();
            CreateMap<GroupInstance, GroupInstanceViewModel>().ReverseMap();
            CreateMap<GetGroupInstanceByIdTeacherQuery, RequestParameter>().ReverseMap();

            CreateMap<LessonInstanceStudent, StudentsByLessonViewModel>()
            .ForMember(destination => destination.FirstName, opts => opts.MapFrom(source => source.Student.FirstName))
            .ForMember(destination => destination.LastName, opts => opts.MapFrom(source => source.Student.LastName));

            CreateMap<Question, GetAllQuestionsViewModel>().ReverseMap();

            // Filter 
            CreateMap<GetAllLevelsQuery, RequestParameter>();
            CreateMap<GetAllTimeSlotsQuery, RequestParameter>();
            CreateMap<GetAllPricingQuery, RequestParameter>();
            CreateMap<GetAllUsersQuery, RequestParameter>();
            CreateMap<GetAllSingleQuestionsQuery, RequestParameter>();
            CreateMap<GetAllQuestionsQuery, RequestParameter>();
            CreateMap<GetAllGroupInstancesQuery, FilteredRequestParameter>();
            CreateMap<GetAllGroupConditionsQuery, FilteredRequestParameter>();

        }
    }
}
