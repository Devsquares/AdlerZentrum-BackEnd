using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EmailType.Queries.GetEmailTypeById
{
    public class GetEmailTypeByIdQuery : IRequest<Response<Domain.Entities.EmailType>>
    {
        public int Id { get; set; }
        public class GetEmailTypeByIdQueryHandler : IRequestHandler<GetEmailTypeByIdQuery, Response<Domain.Entities.EmailType>>
        {
            private readonly IEmailTypeRepositoryAsync _emailtypeRepository;
            public GetEmailTypeByIdQueryHandler(IEmailTypeRepositoryAsync emailtypeRepository)
            {
                _emailtypeRepository = emailtypeRepository;
            }
            public async Task<Response<Domain.Entities.EmailType>> Handle(GetEmailTypeByIdQuery query, CancellationToken cancellationToken)
            {
                var emailtype = await _emailtypeRepository.GetByIdAsync(query.Id);
                if (emailtype == null) throw new ApiException($"EmailType Not Found.");
                return new Response<Domain.Entities.EmailType>(emailtype);
            }
        }
    }
}
