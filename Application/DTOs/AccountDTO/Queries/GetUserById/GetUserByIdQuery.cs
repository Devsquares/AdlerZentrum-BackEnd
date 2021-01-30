using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs 
{
    public class GetUserByIdQuery : IRequest<Response<GetAllUsersViewModel>>
    {
        public string Id { get; set; }
        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Response<GetAllUsersViewModel>>
        {
            private readonly IAccountService _UserRepository;
            private readonly IMapper _mapper;
            public GetUserByIdQueryHandler(IAccountService UserRepository, IMapper mapper)
            {
                _UserRepository = UserRepository;
                _mapper = mapper;
            }
            public async Task<Response<GetAllUsersViewModel>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
            {
                var User = await _UserRepository.GetByIdAsync(query.Id);
                if (User == null) throw new ApiException($"User Not Found.");
               var userViewModel =  _mapper.Map<GetAllUsersViewModel>(User);
                return new Response<GetAllUsersViewModel>(userViewModel);
            }
        }
    }
}
