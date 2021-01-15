using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EmailType.Commands.DeleteEmailTypeById
{
    public class DeleteEmailTypeByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteEmailTypeByIdCommandHandler : IRequestHandler<DeleteEmailTypeByIdCommand, Response<int>>
        {
            private readonly IEmailTypeRepositoryAsync _emailtypeRepository;
            public DeleteEmailTypeByIdCommandHandler(IEmailTypeRepositoryAsync emailtypeRepository)
            {
                _emailtypeRepository = emailtypeRepository;
            }
            public async Task<Response<int>> Handle(DeleteEmailTypeByIdCommand command, CancellationToken cancellationToken)
            {
                var emailtype = await _emailtypeRepository.GetByIdAsync(command.Id);
                if (emailtype == null) throw new ApiException($"EmailType Not Found.");
                await _emailtypeRepository.DeleteAsync(emailtype);
                return new Response<int>(emailtype.Id);
            }
        }
    }
}
