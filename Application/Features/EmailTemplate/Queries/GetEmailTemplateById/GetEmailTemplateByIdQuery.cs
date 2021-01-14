using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EmailTemplate.Queries.GetEmailTemplateById
{
    public class GetEmailTemplateByIdQuery : IRequest<Response<Domain.Entities.EmailTemplate>>
    {
        public int Id { get; set; }
        public class GetEmailTemplateByIdQueryHandler : IRequestHandler<GetEmailTemplateByIdQuery, Response<Domain.Entities.EmailTemplate>>
        {
            private readonly IEmailTemplateRepositoryAsync _emailtemplateRepository;
            public GetEmailTemplateByIdQueryHandler(IEmailTemplateRepositoryAsync emailtemplateRepository)
            {
                _emailtemplateRepository = emailtemplateRepository;
            }
            public async Task<Response<Domain.Entities.EmailTemplate>> Handle(GetEmailTemplateByIdQuery query, CancellationToken cancellationToken)
            {
                var emailtemplate = await _emailtemplateRepository.GetByIdAsync(query.Id);
                if (emailtemplate == null) throw new ApiException($"EmailTemplate Not Found.");
                return new Response<Domain.Entities.EmailTemplate>(emailtemplate);
            }
        }
    }
}
