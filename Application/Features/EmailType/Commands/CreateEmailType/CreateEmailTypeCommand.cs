using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EmailType.Commands.CreateEmailType
{
    public partial class CreateEmailTypeCommand : IRequest<Response<int>>
    {
		public string TypeName { get; set; }
		public string Code { get; set; }
    }

    public class CreateEmailTypeCommandHandler : IRequestHandler<CreateEmailTypeCommand, Response<int>>
    {
        private readonly IEmailTypeRepositoryAsync _emailtypeRepository;
        private readonly IMapper _mapper;
        public CreateEmailTypeCommandHandler(IEmailTypeRepositoryAsync emailtypeRepository, IMapper mapper)
        {
            _emailtypeRepository = emailtypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateEmailTypeCommand request, CancellationToken cancellationToken)
        {
            var emailtype = _mapper.Map<Domain.Entities.EmailType>(request);
            await _emailtypeRepository.AddAsync(emailtype);
            return new Response<int>(emailtype.Id);
        }
    }
}
