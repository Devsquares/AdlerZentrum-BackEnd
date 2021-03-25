using Application.Exceptions;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
            public CreatePermitTeacherCommandHandler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }
            public async Task<Response<bool>> Handle(CreatePermitTeacherCommand command, CancellationToken cancellationToken)
            {
                var user = _userManager.FindByIdAsync(command.TeacherId).Result;
                if(user==null)
                {
                    throw new ApiException("No user found");
                }
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(command.Claim, command.Claim));
                return new Response<bool>(true);
                
            }
        }
    }
}
