using Application.DTOs.Account;
using Application.DTOs;
using Application.DTOs.GroupInstance.Queries;
using Application.DTOs.Level.Commands;
using Application.Enums;
using Application.Filters;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using System;
using Application.Features;
using Domain.Models;
using Application.DTOs.Level.Queries;
using Application.Features.TeacherAbsence.Commands.CreateTeacherAbsence;
using Application.Features.TeacherAbsence.Queries.GetAllTeacherAbsences;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {

            CreateMap<Bug, GetAllBugsViewModel>().ReverseMap();
            CreateMap<CreateBugCommand, Bug>();
            CreateMap<GetAllBugsQuery, GetAllBugsParameter>();

            CreateMap<ForumTopic, GetAllForumTopicsViewModel>()
            .ForMember(destination => destination.ForumCommentsCount, opts => opts.MapFrom(source => source.ForumComments.Count)); ;
            CreateMap<CreateForumTopicCommand, ForumTopic>();
            CreateMap<GetAllForumTopicsQuery, GetAllForumTopicsParameter>();

            CreateMap<ForumReply, GetAllForumReplysViewModel>().ReverseMap();
            CreateMap<CreateForumReplyCommand, ForumReply>();

            CreateMap<ForumComment, GetAllForumCommentsViewModel>()
            .ForMember(destination => destination.ForumReplys, opts => opts.MapFrom(source => source.ForumReplys))
            .ForMember(destination => destination.ForumReplysCount, opts => opts.MapFrom(source => source.ForumReplys.Count));
            CreateMap<CreateForumCommentCommand, ForumComment>();
            CreateMap<GetAllForumCommentsQuery, GetAllForumCommentsParameter>();

            CreateMap<Level, GetAllLevelsViewModel>().ReverseMap();
            CreateMap<Sublevel, SubLevelsViewModel>().ReverseMap();
            CreateMap<LessonDefinition, LessonDefinitionViewModel>().ReverseMap();
            CreateMap<CreateLevelCommand, Level>();
            CreateMap<GetAllLevelsQuery, PagedResponse<Level>>();

            CreateMap<ApplicationUser, GetAllUsersViewModel>().ReverseMap();
            CreateMap<ApplicationUser, AccountViewModel>().ReverseMap();
            CreateMap<GetAllStaffQuery, GetAllUsersParameter>();

            CreateMap<GroupInstance, GetAllGroupInstancesViewModel>().ReverseMap();
            CreateMap<GroupCondition, GetAllGroupConditionViewModel>().ReverseMap();

            CreateMap<TimeSlot, GetAllTimeSlotsViewModel>().ReverseMap();
            CreateMap<TimeSlotDetails, TimeSlotDetailsViewModel>()
            .ForMember(destination => destination.TimeFrom, opts => opts.MapFrom(source => source.TimeFrom.ToString("HH:mm:ss")))
            .ForMember(destination => destination.TimeTo, opts => opts.MapFrom(source => source.TimeTo.ToString("HH:mm:ss")))
            .ReverseMap();
            CreateMap<Pricing, GetAllPricingViewModel>().ReverseMap();
            CreateMap<Test, GetAllTestsViewModel>().ReverseMap().ForMember(destination => destination.TestTypeId,
                 opt => opt.MapFrom(source => Enum.GetName(typeof(TestTypeEnum), source.TestTypeId)));

            CreateMap<SingleQuestion, GetAllSingleQuestionsViewModel>().ReverseMap();
            CreateMap<Question, GetQuestionViewModel>().ReverseMap();
            CreateMap<RequestParameter, GetAllPromoCodesQuery>().ReverseMap();
            CreateMap<PromoCode, GetAllPromoCodesViewModel>().ReverseMap();
            CreateMap<GetAllAdlerCardsUnitsQuery, GetAllAdlerCardsUnitsParameter>().ReverseMap();
            CreateMap<AdlerCardsUnit, GetAllAdlerCardsUnitsViewModel>().ReverseMap();
            CreateMap<GetAllAdlerCardsQuery, GetAllAdlerCardsParameter>().ReverseMap();
            CreateMap<AdlerCard, GetAllAdlerCardsViewModel>().ReverseMap();
            CreateMap<AdlerCardBundleStudent, CreateAdlerCardBundleStudentCommand>().ReverseMap();

            CreateMap<GetAllAdlerCardBundleStudentsQuery, GetAllAdlerCardBundleStudentsParameter>().ReverseMap();
            CreateMap<AdlerCardBundleStudent, GetAllAdlerCardBundleStudentsViewModel>().ReverseMap();
            CreateMap<AdlerCardSubmission, CreateAdlerCardSubmissionCommand>().ReverseMap();


            CreateMap<HomeWorkSubmition, GetAllHomeWorkSubmitionsViewModel>()
            .ForMember(destination => destination.Student, opts => opts.MapFrom(source => source.Student.FirstName + " " + source.Student.LastName))
            .ForMember(destination => destination.GroupInstance, opts => opts.MapFrom(source => source.Homework.GroupInstance.Serial))
            .ForMember(destination => destination.LessonInstance, opts => opts.MapFrom(source => source.Homework.LessonInstance.Serial))
                             .ForMember(destination => destination.Status,
                 opt => opt.MapFrom(source => Enum.GetName(typeof(HomeWorkSubmitionStatusEnum), source.Status)));

            CreateMap<HomeWorkSubmition, GetAllHomeWorkForStudentViewModel>()
                             .ForMember(destination => destination.Status,
                 opt => opt.MapFrom(source => Enum.GetName(typeof(HomeWorkSubmitionStatusEnum), source.Status)));

            CreateMap<TestInstance, TestInstanceToAssginViewModel>()
                       .ForMember(destination => destination.StudentName, opts => opts.MapFrom(source => source.Student.FirstName + " " + source.Student.LastName))
                    .ForMember(destination => destination.GroupSerial, opts => opts.MapFrom(source => source.LessonInstance.GroupInstance.Serial))
                           .ForMember(destination => destination.TestName, opts => opts.MapFrom(source => source.Test.Name))
                  .ForMember(destination => destination.TestType,
                 opt => opt.MapFrom(source => Enum.GetName(typeof(TestTypeEnum), source.Test.TestTypeId)));

            CreateMap<TestInstance, AllTestsToManageViewModel>()
                .ForMember(destination => destination.GroupInstanceSerial, opts => opts.MapFrom(source => source.LessonInstance.GroupInstance.Serial))
                .ForMember(destination => destination.GroupDefinitionSerial, opts => opts.MapFrom(source => source.LessonInstance.GroupInstance.GroupDefinition.Serial))
                .ForMember(destination => destination.TestName, opts => opts.MapFrom(source => source.Test.Name))
                .ForMember(destination => destination.TestId, opts => opts.MapFrom(source => source.Test.Id))
                .ForMember(destination => destination.StudentName, opts => opts.MapFrom(source => source.Student.FirstName + " " + source.Student.LastName))
                .ForMember(destination => destination.GroupInstanceId, opts => opts.MapFrom(source => source.LessonInstance.GroupInstanceId))
                .ForMember(destination => destination.TestType, opts => opts.MapFrom(source => source.Test.TestTypeId));

            CreateMap<Homework, GetAllomeworkBounsViewModel>()
            .ForMember(destination => destination.GroupInstanceSerial, opts => opts.MapFrom(source => source.GroupInstance.Serial))
            .ForMember(destination => destination.TeacherName, opts => opts.MapFrom(source => source.Teacher.FirstName + source.Teacher.LastName))
            .ForMember(destination => destination.LessonOrder, opts => opts.MapFrom(source => source.LessonInstance.Serial))
            .ForMember(destination => destination.SubLevel, opts => opts.MapFrom(source => source.GroupInstance.GroupDefinition.Sublevel.Name))
            .ForMember(destination => destination.LessonInstanceSerial, opts => opts.MapFrom(source => source.LessonInstance.Serial));

            CreateMap<LessonInstance, LessonInstanceViewModel>().ReverseMap();
            CreateMap<LessonInstanceStudent,LessonInstanceStudentViewModel>().ReverseMap();
            CreateMap<GroupInstance, GroupInstanceViewModel>().ReverseMap();
            CreateMap<GetGroupInstanceByIdTeacherQuery, RequestParameter>().ReverseMap();
            CreateMap<TeacherGroupInstanceAssignment, TeacherGroupInstanceViewModel>().ReverseMap();

            CreateMap<LessonInstanceStudent, StudentsByLessonViewModel>()
            .ForMember(destination => destination.FirstName, opts => opts.MapFrom(source => source.Student.FirstName))
            .ForMember(destination => destination.LastName, opts => opts.MapFrom(source => source.Student.LastName));

            CreateMap<Sublevel, GetAllSubLevelsViewModel>().ReverseMap();

            CreateMap<TestInstance, TestInstancesResultsViewModel>()
                .ForMember(destination => destination.StudentName, opts => opts.MapFrom(source => source.Student.FirstName + source.Student.LastName));

            CreateMap<Question, GetAllQuestionsViewModel>().ReverseMap();
            CreateMap<LessonDefinition, GetLessonDefinitionByLevelIdViewModel>().ReverseMap();
            CreateMap<GetAllTestInstancesQuery, GetAllTestInstancesParameter>().ReverseMap();
            CreateMap<GroupDefinition, GetAllGroupDefinitionViewModel>().ReverseMap();
            CreateMap<TestInstance, GetAllTestInstancesViewModel>().ReverseMap();
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
            CreateMap<GetAllBanRequestsQuery, GetAllBanRequestsParameter>().ReverseMap();
            CreateMap<BanRequest, CreateBanRequestCommand>().ReverseMap();
            CreateMap<BanRequest, GetAllBanRequestsViewModel>().ReverseMap();
            CreateMap<TestInstance, TestInstanceViewModel>().ReverseMap();
            CreateMap<DisqualificationRequest, GetAllDisqualificationRequestsViewModel>().ReverseMap();
            CreateMap<GetAllDisqualificationRequestsQuery, GetAllDisqualificationRequestsParameter>().ReverseMap();
            CreateMap<GroupInstanceStudents, GroupInstanceStudentsViewModel>().ReverseMap();
            CreateMap<GetAvailableForRegisterationGroupDefinitionsQuery, RequestParameter>();


            // Adler Cards
            CreateMap<GetAllAdlerCardsBundlesParameter, GetAllAdlerCardsBundlesQuery>().ReverseMap();
            CreateMap<CreateAdlerCardsBundleCommand, AdlerCardsBundle>().ReverseMap();
            CreateMap<GetAllAdlerCardsBundlesViewModel, AdlerCardsBundle>().ReverseMap();
            CreateMap<CreateAdlerCardsUnitCommand, AdlerCardsUnit>().ReverseMap();
            CreateMap<CreateAdlerCardCommand, AdlerCard>().ReverseMap();

            //teacher absence
            CreateMap<CreateTeacherAbsenceCommand, TeacherAbsence>().ReverseMap();
            CreateMap<GetAllTeacherAbsencesViewModel, TeacherAbsence>().ReverseMap();
            CreateMap<GetAllTeacherAbsencesParameter, GetAllTeacherAbsencesQuery>().ReverseMap();
        }
    }
}
