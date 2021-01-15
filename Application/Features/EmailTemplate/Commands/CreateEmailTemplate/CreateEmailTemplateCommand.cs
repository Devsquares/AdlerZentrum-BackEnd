using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EmailTemplate.Commands.CreateEmailTemplate
{
    public partial class CreateEmailTemplateCommand : IRequest<Response<int>>
    {
        public int EmailTypeId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateBody { get; set; }
    }

    public class CreateEmailTemplateCommandHandler : IRequestHandler<CreateEmailTemplateCommand, Response<int>>
    {
        private readonly IEmailTemplateRepositoryAsync _emailtemplateRepository;
        private readonly IMapper _mapper;
        public CreateEmailTemplateCommandHandler(IEmailTemplateRepositoryAsync emailtemplateRepository, IMapper mapper)
        {
            _emailtemplateRepository = emailtemplateRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            var emailtemplate = _mapper.Map<Domain.Entities.EmailTemplate>(request);
            await _emailtemplateRepository.AddAsync(emailtemplate);
            return new Response<int>(emailtemplate.Id);
        }
    }
}
