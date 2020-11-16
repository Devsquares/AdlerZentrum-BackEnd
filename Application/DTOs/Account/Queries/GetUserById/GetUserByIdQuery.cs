using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.Account.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<Response<Domain.Entities.Account>>
    {
        public string Id { get; set; }
        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Response<Domain.Entities.Account>>
        {
            private readonly IAccountService _UserRepository;
            public GetUserByIdQueryHandler(IAccountService UserRepository)
            {
                _UserRepository = UserRepository;
            }
            public async Task<Response<Domain.Entities.Account>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
            {
                var User = await _UserRepository.GetByIdAsync(query.Id);
                if (User == null) throw new ApiException($"User Not Found.");
                return new Response<Domain.Entities.Account>(User);
            }
        }
    }
}
