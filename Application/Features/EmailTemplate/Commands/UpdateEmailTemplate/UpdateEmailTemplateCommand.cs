using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class UpdateEmailTemplateCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int EmailTypeId { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }

        public class UpdateEmailTemplateCommandHandler : IRequestHandler<UpdateEmailTemplateCommand, Response<int>>
        {
            private readonly IEmailTemplateRepositoryAsync _emailtemplateRepository;
            public UpdateEmailTemplateCommandHandler(IEmailTemplateRepositoryAsync emailtemplateRepository)
            {
                _emailtemplateRepository = emailtemplateRepository;
            }
            public async Task<Response<int>> Handle(UpdateEmailTemplateCommand command, CancellationToken cancellationToken)
            {
                var emailtemplate = await _emailtemplateRepository.GetByIdAsync(command.Id);

                if (emailtemplate == null)
                {
                    throw new ApiException($"EmailTemplate Not Found.");
                }
                else
                {
                    emailtemplate.EmailTypeId = command.EmailTypeId;
                    emailtemplate.Name = command.Name;
                    emailtemplate.Body = command.Body;
                    emailtemplate.Subject = command.Subject;

                    await _emailtemplateRepository.UpdateAsync(emailtemplate);
                    return new Response<int>(emailtemplate.Id);
                }
            }
        }

    }
}
