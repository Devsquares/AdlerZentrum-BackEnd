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
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Process
{
    public class SendMailThread
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MailWorker> _logger;
        private readonly IEmailService _emailService;

        public SendMailThread(IServiceProvider serviceProvider, ILogger<MailWorker> logger, IEmailService mailservice)
        {
            _serviceProvider = serviceProvider;
            _emailService = mailservice;
            _logger = logger;
        }
        public void Run(object job)
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

                var template = dbContext.Set<EmailTemplate>().Where(x => x.EmailTypeId == _job.Type).AsNoTracking().FirstOrDefault();
                if (template == null)
                {
                    _logger.LogError("Email Template is empty");
                    return;
                }
                try
                {
                    string to = FormatMail(_job, dbContext, template);

                    var task = _emailService.SendAsync(new Application.DTOs.Email.EmailRequest()
                    {
                        To = to,
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

        private  string FormatMail(MailJob _job, ApplicationDbContext dbContext, EmailTemplate template)
        {
            string to = "";
            switch ((MailJobTypeEnum)_job.Type)
            {
                case MailJobTypeEnum.Banning:
                case MailJobTypeEnum.Registeration:
                case MailJobTypeEnum.Disqualification:
                case MailJobTypeEnum.DownGrading:
                    var user1 = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.StudentId).FirstOrDefault();
                    template.Body = template.Body.FormatWith(user1);
                    to = user1.Email;
                    break;
                case MailJobTypeEnum.SendMessageToInstructor:
                case MailJobTypeEnum.SendMessageToAdmin:
                    // TODO: need to defne.
                    var user = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.StudentId).FirstOrDefault();
                    template.Body = template.Body.FormatWith(user);
                    break;
                case MailJobTypeEnum.GroupActivationStudent:
                    var studentGroupActivetion = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.StudentId).FirstOrDefault();
                    var groupActivationStudent = dbContext.Set<GroupInstance>().Where(x => x.Id == _job.GroupInstanceId).FirstOrDefault();
                    var input = new
                    {
                        FirstName = studentGroupActivetion.FirstName,
                        LastName = studentGroupActivetion.LastName,
                        Serial = groupActivationStudent.Serial
                    };
                    template.Body = template.Body.FormatWith(input);
                    to = studentGroupActivetion.Email;
                    break;
                case MailJobTypeEnum.GroupActivationTeacher:
                    var teacher = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.TeacherId).FirstOrDefault();
                    var groupActivationTeacher = dbContext.Set<GroupInstance>().Where(x => x.Id == _job.GroupInstanceId).FirstOrDefault();
                    var inputGroupActivationTeacher = new
                    {
                        FirstName = teacher.FirstName,
                        LastName = teacher.LastName,
                        Serial = groupActivationTeacher.Serial
                    };
                    template.Body = template.Body.FormatWith(inputGroupActivationTeacher);
                    to = teacher.Email;
                    break;
                case MailJobTypeEnum.HomeworkAssignment:
                    break;
                case MailJobTypeEnum.TestCorrected:
                case MailJobTypeEnum.TestSubmitted:
                    var TestCorrectedUser = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.StudentId).FirstOrDefault();
                    var test = dbContext.Set<TestInstance>().Include(x => x.Test).Where(x => x.Id == _job.TestInstanceId).FirstOrDefault();
                    var inputTestCorrected = new
                    {
                        FirstName = TestCorrectedUser.FirstName,
                        LastName = TestCorrectedUser.LastName,
                        Name = test.Test.Name
                    };
                    template.Body = template.Body.FormatWith(inputTestCorrected);
                    to = TestCorrectedUser.Email;
                    break;
                case MailJobTypeEnum.HomeworkCorrected:
                    var HomeworkUser = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.StudentId).FirstOrDefault();

                    var inputHomework = new
                    {
                        FirstName = HomeworkUser.FirstName,
                        LastName = HomeworkUser.LastName,
                        Id = _job.HomeworkId
                    };
                    template.Body = template.Body.FormatWith(inputHomework);
                    to = HomeworkUser.Email;
                    break;
                case MailJobTypeEnum.HomeworkSubmitted:
                    var HomeworkSubmittedUser = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.StudentId).FirstOrDefault();
                    var HomeworkSubmittedTeacher = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.TeacherId).FirstOrDefault();


                    var inputHomeworkSubmitted = new
                    {
                        FirstName = HomeworkSubmittedUser.FirstName,
                        LastName = HomeworkSubmittedUser.LastName,
                        Id = _job.HomeworkId
                    };
                    template.Body = template.Body.FormatWith(inputHomeworkSubmitted);
                    to = HomeworkSubmittedTeacher?.Email;
                    break;
                case MailJobTypeEnum.ContactUs:

                    var ContactUs = new
                    {
                        Email = _job.Email,
                        Subject = _job.Subject,
                        Message = _job.Message
                    };
                    template.Body = template.Body.FormatWith(ContactUs);
                    to = _emailService.GetMail();
                    break;
                case MailJobTypeEnum.RejectTeacherAbsence:
                    var rejectedteacher = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.TeacherId).FirstOrDefault();
                    var reject = "Your Request has been rejected";
                    template.Body = template.Body.FormatWith(reject);
                    to = rejectedteacher.Email;
                    break;
                case MailJobTypeEnum.AcceptTeacherAbsence:
                    var acceptedteacher = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.TeacherId).FirstOrDefault();
                    var accept = "Your Request has been Accepted";
                    template.Body = template.Body.FormatWith(accept);
                    to = acceptedteacher.Email;
                    break;
                case MailJobTypeEnum.AcceptTeacherAbsenceWithAnotherTeacher:
                    var acceptedNewteacher = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.TeacherId).FirstOrDefault();
                    var assigninglesson = "You have a new assigned lesson";
                    template.Body = template.Body.FormatWith(assigninglesson);
                    to = acceptedNewteacher.Email;
                    break;
                    // todo check superVisor
                case MailJobTypeEnum.RequestAbsenceToSuperVisor:
                    var requestteacher = dbContext.Set<ApplicationUser>().Where(x => x.Id == _job.TeacherId).FirstOrDefault();
                    var absenceRequest = $"You have a new Absence Request from Teacher :{requestteacher.FirstName}{requestteacher.LastName}";
                    template.Body = template.Body.FormatWith(absenceRequest);
                    to = _emailService.GetMail();
                    break;
                default:
                    break;
            }
            return to;
        }

        public static SendMailThread Create(IServiceProvider serviceProvider, ILogger<MailWorker> logger, IEmailService emailService)
        {
            return new SendMailThread(serviceProvider, logger, emailService);
        }
    }
}