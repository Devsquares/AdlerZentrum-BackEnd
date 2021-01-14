using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EmailTemplate.Commands.DeleteEmailTemplateById
{
    public class DeleteEmailTemplateByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteEmailTemplateByIdCommandHandler : IRequestHandler<DeleteEmailTemplateByIdCommand, Response<int>>
        {
            private readonly IEmailTemplateRepositoryAsync _emailtemplateRepository;
            public DeleteEmailTemplateByIdCommandHandler(IEmailTemplateRepositoryAsync emailtemplateRepository)
            {
                _emailtemplateRepository = emailtemplateRepository;
            }
            public async Task<Response<int>> Handle(DeleteEmailTemplateByIdCommand command, CancellationToken cancellationToken)
            {
                var emailtemplate = await _emailtemplateRepository.GetByIdAsync(command.Id);
                if (emailtemplate == null) throw new ApiException($"EmailTemplate Not Found.");
                await _emailtemplateRepository.DeleteAsync(emailtemplate);
                return new Response<int>(emailtemplate.Id);
            }
        }
    }
}
