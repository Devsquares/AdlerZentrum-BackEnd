using Application.Filters;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllStaffQuery : IRequest<UserPagedResponse<IEnumerable<GetAllUsersViewModel>>>
    {
        public string Role { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllStaffQueryHandler : IRequestHandler<GetAllStaffQuery, UserPagedResponse<IEnumerable<GetAllUsersViewModel>>>
    {
        private readonly IAccountService _userRepository;
        private readonly IMapper _mapper;
        public GetAllStaffQueryHandler(IAccountService userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserPagedResponse<IEnumerable<GetAllUsersViewModel>>> Handle(GetAllStaffQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllUsersParameter>(request);
            int count = 0;
            var user = _userRepository.GetPagedReponseStaffAsync(validFilter.PageNumber, validFilter.PageSize, request.Role, out count);
            var userViewModel = _mapper.Map<IEnumerable<GetAllUsersViewModel>>(user);
            return new UserPagedResponse<IEnumerable<GetAllUsersViewModel>>(userViewModel, validFilter.Role, validFilter.PageNumber, validFilter.PageSize, count);
        }
    }
}
