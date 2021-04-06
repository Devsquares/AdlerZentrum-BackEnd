using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features 
{
    public class CreatePermitTeacherCommand : IRequest<Response<bool>>
    {
       public string TeacherId { get; set; }
        public string Claim { get; set; }
        public class CreatePermitTeacherCommandHandler : IRequestHandler<CreatePermitTeacherCommand, Response<bool>>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IClaimRepositoryAsync _claimRepositoryAsync;
            public CreatePermitTeacherCommandHandler(UserManager<ApplicationUser> userManager, IClaimRepositoryAsync claimRepositoryAsync)
            {
                _userManager = userManager;
                _claimRepositoryAsync = claimRepositoryAsync;
            }
            public async Task<Response<bool>> Handle(CreatePermitTeacherCommand command, CancellationToken cancellationToken)
            {
                var user = _userManager.FindByIdAsync(command.TeacherId).Result;
                if(user==null)
                {
                    throw new ApiException("No user found");
                }
                //Claim filterclaim = new Claim(command.Claim, command.Claim);
                //var users = _userManager.GetUsersForClaimAsync(filterclaim).Result;
                //var oneUser = users.Where(x => x.Id == command.TeacherId).FirstOrDefault();
                var isFound = _claimRepositoryAsync.CheckUserClaims(command.TeacherId, command.Claim);
                if (!isFound)
                {
                    throw new ApiException("this claim was added befor for this teacher");
                }
                await _userManager.AddClaimAsync(user, new Claim(command.Claim, command.Claim));
                return new Response<bool>(true);
                
            }
        }
    }
}
