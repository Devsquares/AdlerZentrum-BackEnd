﻿using Application.DTOs.Account;
using Application.DTOs;
using Application.DTOs.GroupInstance.Queries;
using Application.DTOs.Level.Commands;
using Application.Enums;
using Application.Filters;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using System;
using Application.Features.TestInstance.Queries.GetAllTestInstances;

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

            CreateMap<HomeWorkSubmition, GetAllHomeWorkSubmitionsViewModel>()
            .ForMember(destination => destination.Student, opts => opts.MapFrom(source => source.Student.FirstName + " " + source.Student.LastName))
            .ForMember(destination => destination.GroupInstance, opts => opts.MapFrom(source => source.Homework.GroupInstance.Serial))
            .ForMember(destination => destination.LessonInstance, opts => opts.MapFrom(source => source.Homework.LessonInstance.Serial))
                             .ForMember(destination => destination.Status,
                 opt => opt.MapFrom(source => Enum.GetName(typeof(HomeWorkSubmitionStatusEnum), source.Status)));

            CreateMap<HomeWorkSubmition, GetAllHomeWorkForStudentViewModel>()
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

            CreateMap<Sublevel, GetAllSubLevelsViewModel>()
                .ForMember(destination => destination.Name, opts => opts.MapFrom(source => source.Level.Name + "." + source.Name));

            CreateMap<Question, GetAllQuestionsViewModel>().ReverseMap();
            CreateMap<LessonDefinition, GetLessonDefinitionByLevelIdViewModel>().ReverseMap();
            CreateMap<GetAllTestInstancesQuery, GetAllTestInstancesParameter>().ReverseMap();
            CreateMap<GroupDefinition, GetAllGroupDefinitionViewModel>().ReverseMap();
            CreateMap<TestInstance, GetAllTestInstancesViewModel>().ReverseMap();
            // Filter 
            CreateMap<GetAllLevelsQuery, RequestParameter>();
            CreateMap<GetAllTestsQuery, RequestParameter>();
            CreateMap<GetAllGroupDefinitionsQuery, RequestParameter>().ReverseMap();
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
