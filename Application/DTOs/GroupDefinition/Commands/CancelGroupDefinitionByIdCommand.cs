using Application.DTOs.Email;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CancelGroupDefinitionByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public class CancelGroupDefinitionByIdCommandHandler : IRequestHandler<CancelGroupDefinitionByIdCommand, Response<int>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            private readonly IEmailTemplateRepositoryAsync _EmailTemplateRepository;
            private readonly IEmailService _EmailService;
            private readonly IGroupInstanceStudentRepositoryAsync _GroupInstanceStudentRepository;
            private readonly IUsersRepositoryAsync _UsersRepository;
            public CancelGroupDefinitionByIdCommandHandler(IGroupDefinitionRepositoryAsync GroupDefinition,
                IEmailTemplateRepositoryAsync EmailTemplateRepository,
                IEmailService EmailService,
                IGroupInstanceStudentRepositoryAsync GroupInstanceStudentRepository,
                IUsersRepositoryAsync UsersRepository)
            {
                _GroupDefinitionRepository = GroupDefinition;
                _EmailTemplateRepository = EmailTemplateRepository;
                _EmailService = EmailService;
                _GroupInstanceStudentRepository = GroupInstanceStudentRepository;
                _UsersRepository = UsersRepository;
            }
            public async Task<Response<int>> Handle(CancelGroupDefinitionByIdCommand command, CancellationToken cancellationToken)
            {
                var groupDefinition = await _GroupDefinitionRepository.GetByIdAsync(command.Id);
                if (groupDefinition == null) throw new ApiException($"Group Not Found.");

                // send mail.
                var emailTemplates = _EmailTemplateRepository.GetEmailTemplateByEmailTypeId((int)EmailTypeEnum.Cancel);
                if (emailTemplates == null || (emailTemplates != null && emailTemplates.Count == 0)) throw new ApiException($"Cancel Email Template Not Found.");
                var studentEmails = _GroupInstanceStudentRepository.GetEmailsByGroupDefinationId(command.Id);
                var user = _UsersRepository.GetUserById(command.UserId);
                EmailRequest emailRequest = new EmailRequest();
                emailRequest.Body = emailTemplates[0].Body;
                emailRequest.Subject = "Cancel Group defination";
                emailRequest.From = user.Email;
                for (int i = 0; i < studentEmails.Count; i++)
                {
                    emailRequest.To += studentEmails[i] + ";";
                }
                await _EmailService.SendAsync(emailRequest);

                groupDefinition.Status = (int)GroupDefinationStatusEnum.Canceld;
                await _GroupDefinitionRepository.UpdateAsync(groupDefinition);
                return new Response<int>(groupDefinition.Id);
            }
        }
    }
}
