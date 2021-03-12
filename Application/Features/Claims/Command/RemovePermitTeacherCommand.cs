using Application.Exceptions;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Claims.Command
{
    public class RemovePermitTeacherCommand : IRequest<Response<bool>>
    {
       public string TeacherId { get; set; }
        public string Claim { get; set; }
        public class RemovePermitTeacherCommandHandler : IRequestHandler<RemovePermitTeacherCommand, Response<bool>>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            public RemovePermitTeacherCommandHandler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }
            public async Task<Response<bool>> Handle(RemovePermitTeacherCommand command, CancellationToken cancellationToken)
            {
                var user = _userManager.FindByIdAsync(command.TeacherId).Result;
                if (user == null)
                {
                    throw new ApiException("No user found");
                }
                Claim filterclaim = new Claim(command.Claim, command.Claim);

                await _userManager.RemoveClaimAsync(user, filterclaim);
                return new Response<bool>(true);
                
            }
        }
    }
}
