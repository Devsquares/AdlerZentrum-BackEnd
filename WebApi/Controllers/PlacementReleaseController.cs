using Application.DTOs;
using Application.Features;
using Application.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class PlacementReleaseController : BaseApiController
    {

        [HttpPost("Create")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Post(CreatePlacementReleaseCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("PlacementUnRelease")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> PlacementUnRelease(CreatePlacementUnReleaseCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


    }
}
