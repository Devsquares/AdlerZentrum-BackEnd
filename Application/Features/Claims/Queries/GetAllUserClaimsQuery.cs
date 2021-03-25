using Domain.Entities;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features 
{
    public class GetAllUserClaimsQuery : IRequest<IEnumerable<UserClaimsModel>>
    {
        public string ClaimType { get; set; }
        public class GetAllUserClaimsQueryHandler : IRequestHandler<GetAllUserClaimsQuery, IEnumerable<UserClaimsModel>>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            public GetAllUserClaimsQueryHandler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }
            public async Task<IEnumerable<UserClaimsModel>> Handle(GetAllUserClaimsQuery command, CancellationToken cancellationToken)
            {
                Claim filterclaim = new Claim(command.ClaimType, command.ClaimType);
               var users =  _userManager.GetUsersForClaimAsync(filterclaim).Result;
                UserClaimsModel userClaimsModel = new UserClaimsModel();
                List<UserClaimsModel> userClaimsModellist = new List<UserClaimsModel>();
                foreach (var user in users)
                {
                    userClaimsModel = new UserClaimsModel();
                    userClaimsModel.FirstName = user.FirstName;
                    userClaimsModel.LastName = user.LastName;
                    userClaimsModel.userId = user.Id;
                    userClaimsModel.Email = user.Email;
                    userClaimsModellist.Add(userClaimsModel);
                }
                return userClaimsModellist;
            }
        }
    }
}
