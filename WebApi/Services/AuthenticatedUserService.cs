using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("sid");
            Role = httpContextAccessor.HttpContext?.User?.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            GroupInstanceId = null;
            try
            {
                GroupInstanceId = Convert.ToInt32(httpContextAccessor.HttpContext?.User?.FindFirstValue("GroupInstance"));
            }
            catch
            {
            }
        }

        public string UserId { get; }

        public string Role { get; }

        public int? GroupInstanceId { get; }
    }
}
