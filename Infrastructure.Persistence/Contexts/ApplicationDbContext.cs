using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Application.Enums;

namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Sublevel> SubLevels { get; set; }
        public DbSet<GroupInstance> GroupInstances { get; set; }
        public DbSet<TeacherGroupInstanceAssignment> TeacherGroupInstances { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<LessonInstance> LessonInstances { get; set; }
        public DbSet<HomeWorkSubmition> HomeWorkSubmitions { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestInstance> TestInstances { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<SingleQuestion> SingleQuestions { get; set; }
        public DbSet<SingleQuestionSubmission> SingleQuestionSubmissions { get; set; }
        public DbSet<ChoiceSubmission> ChoiceSubmissions { get; set; }
        public DbSet<BanRequest> BanRequests { get; set; }
        public DbSet<LessonInstanceStudent> LessonInstanceStudents { get; set; }
        public DbSet<EmailType> EmailTypes { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<GroupConditionDetail> GroupConditionDetails { get; set; }
        public DbSet<GroupConditionPromoCode> GroupConditionPromoCodes { get; set; }
        public DbSet<InterestedStudent> InterestedStudents { get; set; }
        public DbSet<OverPaymentStudent> OverPaymentStudents { get; set; }
        public DbSet<DisqualificationRequest> DisqualificationRequests { get; set; }
        public DbSet<GroupInstanceStudents> GroupInstanceStudents { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<PromoCodeInstance> PromoCodeInstances { get; set; }
        public DbSet<AdlerCard> AdlerCards { get; set; }
        public DbSet<AdlerCardsBundle> AdlerCardsBundles { get; set; }
        public DbSet<AdlerCardSubmission> AdlerCardSubmissions { get; set; }
        public DbSet<AdlerCardsUnit> AdlerCardsUnits { get; set; }
        public DbSet<AdlerCardBundleStudent> AdlerCardBundleStudents { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "ApplicationUsers");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });


            builder.Entity<ApplicationUser>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<ApplicationUser>(entity => entity.Property(m => m.NormalizedEmail).HasMaxLength(85));
            builder.Entity<ApplicationUser>(entity => entity.Property(m => m.NormalizedUserName).HasMaxLength(85));

            builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.NormalizedName).HasMaxLength(85));

            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(85));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));

            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));

            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(85));

            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));

        }
    }
}
