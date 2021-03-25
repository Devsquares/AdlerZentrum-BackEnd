using Application.DTOs;
using Application.DTOs.Level.Queries;
using Application.Exceptions;
using Application.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ClaimController : BaseApiController
    {
        [HttpGet("GetAllClaims")]
        public async Task<IActionResult> GetAllClaims()
        {
            return Ok(await Mediator.Send(new GetAllClaimsQuery()));
        }

        [HttpPost("PermitTeacher")]
        public async Task<IActionResult> PermitTeacher([FromQuery] CreatePermitTeacherCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("GetPermittedTeachers")]
        public async Task<IActionResult> GetPermittedTeachers([FromQuery] GetAllUserClaimsQuery command)
        {
            if(string.IsNullOrEmpty(command.ClaimType))
            {
                throw new ApiException("Please Enter Claim type");
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("RemoveTeacherPermit")]
        public async Task<IActionResult> RemoveTeacherPermit([FromQuery] RemovePermitTeacherCommand command)
        {
            return Ok(await Mediator.Send(command));
        } 

        [HttpPost("GetNonPermittedTeachers")]
        public async Task<IActionResult> GetNonPermittedTeachers([FromQuery] GetNonAllUserClaimsQuery command)
        {
            if(command.PageNumber == 0)
            {
                command.PageNumber = 1;
            }
            if (command.PageSize == 0)
            {
                command.PageSize = 10;
            }
            return Ok(await Mediator.Send(command));
        }

    }
}
