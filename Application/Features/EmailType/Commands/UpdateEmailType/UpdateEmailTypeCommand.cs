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
	public class UpdateEmailTypeCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public string TypeName { get; set; }
		public string Code { get; set; }

        public class UpdateEmailTypeCommandHandler : IRequestHandler<UpdateEmailTypeCommand, Response<int>>
        {
            private readonly IEmailTypeRepositoryAsync _emailtypeRepository;
            public UpdateEmailTypeCommandHandler(IEmailTypeRepositoryAsync emailtypeRepository)
            {
                _emailtypeRepository = emailtypeRepository;
            }
            public async Task<Response<int>> Handle(UpdateEmailTypeCommand command, CancellationToken cancellationToken)
            {
                var emailtype = await _emailtypeRepository.GetByIdAsync(command.Id);

                if (emailtype == null)
                {
                    throw new ApiException($"EmailType Not Found.");
                }
                else
                {
				emailtype.TypeName = command.TypeName;
				emailtype.Code = command.Code; 

                    await _emailtypeRepository.UpdateAsync(emailtype);
                    return new Response<int>(emailtype.Id);
                }
            }
        }

    }
}
