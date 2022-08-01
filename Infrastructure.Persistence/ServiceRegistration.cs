using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Domain.Settings;
using Newtonsoft.Json;
using Application.Wrappers;
using Domain.Entities;
using System.Net;
using Serilog;

namespace Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationDbContext>(options =>
               options.UseMySql(
                   connectionString,
                   ServerVersion.AutoDetect(connectionString),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IBugRepositoryAsync, BugRepositoryAsync>();
            services.AddTransient<ILevelRepositoryAsync, LevelRepositoryAsync>();
            services.AddTransient<ISublevelRepositoryAsync, SubLevelRepositoryAsync>();
            services.AddTransient<IGroupInstanceRepositoryAsync, GroupInstanceRepositoryAsync>();
            services.AddTransient<ITeacherGroupInstanceAssignmentRepositoryAsync, TeacherGroupInstanceAssignmentRepositoryAsync>();
            services.AddTransient<IPromoCodeRepositoryAsync, PromoCodeRepositoryAsync>();
            services.AddTransient<IGroupConditionRepositoryAsync, GroupConditionRepositoryAsync>();
            services.AddTransient<ILessonInstanceRepositoryAsync, LessonInstanceRepositoryAsync>();
            services.AddTransient<ILessonInstanceStudentRepositoryAsync, LessonInstanceStudentRepositoryAsync>();
            services.AddTransient<IHomeWorkSubmitionRepositoryAsync, HomeWorkSubmitionRepositoryAsync>();
            services.AddTransient<ITimeSlotRepositoryAsync, TimeSlotRepositoryAsync>();
            services.AddTransient<IHomeWorkRepositoryAsync, HomeWorkRepositoryAsync>();
            services.AddTransient<IPricingRepositoryAsync, PricingRepositoryAsync>();
            services.AddTransient<ISingleQuestionRepositoryAsync, SingleQuestionRepositoryAsync>();
            services.AddTransient<IQuestionRepositoryAsync, QuestionRepositoryAsync>();
            services.AddTransient<ILessonDefinitionRepositoryAsync, LessonDefinitionRepositoryAsync>();
            services.AddTransient<ITestRepositoryAsync, TestRepositoryAsync>();
            services.AddTransient<IGroupDefinitionRepositoryAsync, GroupDefinitionRepositoryAsync>();
            services.AddTransient<ITestRepositoryAsync, TestRepositoryAsync>();
            services.AddTransient<ITestInstanceRepositoryAsync, TestInstanceRepositoryAsync>();
            services.AddTransient<IGroupInstanceStudentRepositoryAsync, GroupInstanceStudentRepositoryAsync>();
            services.AddTransient<ISingleQuestionSubmissionRepositoryAsync, SingleQuestionSubmissionRepositoryAsync>();
            services.AddTransient<IChoiceSubmissionRepositoryAsync, ChoiceSubmissionRepositoryAsync>();
            services.AddTransient<IBanRequestRepositoryAsync, BanRequestRepositoryAsync>();
            services.AddTransient<IEmailTemplateRepositoryAsync, EmailTemplateRepositoryAsync>();
            services.AddTransient<IUsersRepositoryAsync, UsersRepositoryAsync>();
            services.AddTransient<IGroupConditionDetailsRepositoryAsync, GroupConditionDetailsRepositoryAsync>();
            services.AddTransient<IGroupConditionPromoCodeRepositoryAsync, GroupConditionPromoCodeRepositoryAsync>();
            services.AddTransient<IInterestedStudentRepositoryAsync, InterestedStudentRepositoryAsync>();
            services.AddTransient<IOverPaymentStudentRepositoryAsync, OverPaymentStudentRepositoryAsync>();
            services.AddTransient<IDisqualificationRequestRepositoryAsync, DisqualificationRequestRepositoryAsync>();
            services.AddTransient<IJobRepositoryAsync, JobRepositoryAsync>();
            services.AddTransient<IPromoCodeInstanceRepositoryAsync, PromoCodeInstanceRepositoryAsync>();
            services.AddTransient<IAdlerCardRepositoryAsync, AdlerCardRepositoryAsync>();
            services.AddTransient<IAdlerCardsBundleRepositoryAsync, AdlerCardsBundleRepositoryAsync>();
            services.AddTransient<IAdlerCardSubmissionRepositoryAsync, AdlerCardSubmissionRepositoryAsync>();
            services.AddTransient<IAdlerCardsUnitRepositoryAsync, AdlerCardsUnitRepositoryAsync>();
            services.AddTransient<IAdlerCardBundleStudentRepositoryAsync, AdlerCardBundleStudentRepositoryAsync>();
            services.AddTransient<IForumTopicRepositoryAsync, ForumTopicRepositoryAsync>();
            services.AddTransient<IForumCommentRepositoryAsync, ForumCommentRepositoryAsync>();
            services.AddTransient<IForumReplyRepositoryAsync, ForumReplyRepositoryAsync>();
            services.AddTransient<IPlacementReleaseReopsitoryAsync, PlacementReleaseReopsitoryAsync>();
            services.AddTransient<IStudentInfoRepositoryAsync, StudentInfoRepositoryAsync>();
            services.AddTransient<ISettingRepositoryAsync, SettingRepositoryAsync>();
            services.AddTransient<IMailJobRepositoryAsync, MailJobRepositoryAsync>();
            services.AddTransient<IClaimRepositoryAsync, ClaimRepositoryAsync>();
            services.AddTransient<ITeacherAbsenceRepositoryAsync, TeacherAbsenceRepositoryAsync>();
            services.AddTransient<IDuplicateExceptionRepositoryAsync, DuplicateExceptionRepositoryAsync>();
            services.AddTransient<IPaymentTransactionsRepositoryAsync, PaymentTransactionsRepositoryAsync>();
            #endregion

            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            #region Services
            services.AddTransient<IAccountService, AccountService>();
            #endregion

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            Log.Information(configuration["JWTSettings"]);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            c.Response.ContentType = "application/json";
                            if (c.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                c.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                                c.Response.Headers.Add("Error", "7amada");
                            }
                            var result = JsonConvert.SerializeObject(new Response<string>(c.Exception.ToString()));
                            return c.Response.WriteAsync(result);
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }

    }
}
