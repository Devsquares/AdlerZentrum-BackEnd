using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Claims.Queries
{
    public class GetNonAllUserClaimsQuery : IRequest<PagedResponse<IEnumerable<UserClaimsModel>>>
    {
        public string ClaimType { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public class GetNonAllUserClaimsQueryHandler : IRequestHandler<GetNonAllUserClaimsQuery, PagedResponse<IEnumerable<UserClaimsModel>>>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IAccountService _accountService;
            public GetNonAllUserClaimsQueryHandler(UserManager<ApplicationUser> userManager, IAccountService accountService)
            {
                _userManager = userManager;
                _accountService = accountService;
            }
            public async Task<PagedResponse<IEnumerable<UserClaimsModel>>> Handle(GetNonAllUserClaimsQuery command, CancellationToken cancellationToken)
            {
                return await _accountService.GetNonAllUserClaims(command.PageNumber, command.PageSize, command.Email, command.Name, command.ClaimType);
               
            }
        }
    }
}
