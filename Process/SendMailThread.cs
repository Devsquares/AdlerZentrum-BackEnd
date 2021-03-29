using System;
using System.Linq;
using Application.Enums;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Application.Features;
using Microsoft.Extensions.Logging;
using FormatWith;
using Infrastructure.Shared.Services;

namespace Process
{
    public class SendMailThread
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MailWorker> _logger;
        private readonly EmailService _emailService;

        public SendMailThread(IServiceProvider serviceProvider, ILogger<MailWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public  void Run(object job)
        {
            MailJob _job = (MailJob)job;
            using (var scope = _serviceProvider.CreateScope())
            {
                // update job to running.
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                _job.Status = (int)JobStatusEnum.Running;
                _job.ExecutionDate = DateTime.Now;
                dbContext.Update(_job);
                dbContext.SaveChanges();

                var template = dbContext.Set<EmailTemplate>().Where(x => x.EmailTypeId == _job.Type).FirstOrDefault();
                if (template == null)
                {
                    _logger.LogError("Email Template is empty");
                    return;
                }
                try
                {
                    FormatMail(_job, dbContext, template);

                    var task = _emailService.SendAsync(new Application.DTOs.Email.EmailRequest()
                    {
                        To = _job.To,
                        Body = template.Body,
                        Subject = template.Subject
                    });

                    _job.Status = (int)JobStatusEnum.Done;
                    _job.FinishDate = DateTime.Now;
                }
                catch (System.Exception ex)
                {
                    _job.Failure = ex.Message;
                    _job.Status = (int)JobStatusEnum.Failed;
                    dbContext.Update(_job);
                    dbContext.SaveChanges();
                }

                dbContext.Update(_job);
                dbContext.SaveChanges();
            }
        }

        private static EmailTemplate FormatMail(MailJob _job, ApplicationDbContext dbContext, EmailTemplate template)
        {
            switch ((MailJobTypeEnum)_job.Type)
            {
                case MailJobTypeEnum.Banning:
                case MailJobTypeEnum.Registeration:
                case MailJobTypeEnum.Disqualification:
                case MailJobTypeEnum.DownGrading:
                case MailJobTypeEnum.SendMessageToInstructor:
                case MailJobTypeEnum.SendMessageToAdmin:
                    var user = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.StudentId).FirstOrDefault();
                    template.Body = template.Body.FormatWith(user);
                    break;
                case MailJobTypeEnum.GroupActivation:
                    break;
                case MailJobTypeEnum.HomeworkAssignment:
                    break;
                case MailJobTypeEnum.TestCorrected:
                    break;
                case MailJobTypeEnum.HomeworkCorrected:
                    break;
                case MailJobTypeEnum.HomeworkSubmitted:
                    break;
                case MailJobTypeEnum.TestSubmitted:
                    break;
                default:
                    break;
            }
            return template;
        }

        public static SendMailThread Create(IServiceProvider serviceProvider, ILogger<MailWorker> logger)
        {
            return new SendMailThread(serviceProvider, logger);
        }
    }
}